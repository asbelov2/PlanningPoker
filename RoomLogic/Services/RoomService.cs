using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.SignalR;

namespace RoomApi
{
  /// <summary>
  /// RoomService class. Contains methods for working with rooms.
  /// </summary>
  public class RoomService
  {
    private IHubContext<RoomHub> context;
    private UserService userService;
    private RoundService roundService;
    private RoomRepository rooms;
    private RoundRepository rounds;
    private RoundTimerService timers;
    private UsersReadinessService isUsersReady;
    private DeckService deckService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomService"/> class.
    /// </summary>
    /// <param name="hubContext">Hub context.</param>
    /// <param name="roundService">Round service.</param>
    /// <param name="userService">User service.</param>
    /// <param name="roomRepository">Room repository.</param>
    /// <param name="roundRepository">Round repository.</param>
    /// <param name="roundTimerService">Round timer service.</param>
    /// <param name="usersReadinessService">User readiness service.</param>
    /// <param name="roundResultRepository">Round result repository.</param>
    public RoomService(
      IHubContext<RoomHub> hubContext,
      RoundService roundService,
      UserService userService,
      DeckService deckService,
      RoomRepository roomRepository,
      RoundRepository roundRepository,
      RoundTimerService roundTimerService,
      UsersReadinessService usersReadinessService)
    {
      this.context = hubContext;
      this.roundService = roundService;
      this.userService = userService;
      this.deckService = deckService;
      this.rooms = roomRepository;
      this.rounds = roundRepository;
      this.timers = roundTimerService;
      this.isUsersReady = usersReadinessService;
    }

    /// <summary>
    /// Starts new round.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="deck">Deck.</param>
    /// <param name="title">Title.</param>
    /// <param name="roundTime">Round time.</param>
    /// <returns>Round ID.</returns>
    public Guid StartNewRound(Guid roomId, Guid userId, Deck deck, string title = "Default title", TimeSpan roundTime = default)
    {
      if (this.IsHost(userId, roomId) && (this.rooms.GetItem(roomId) != null))
      {
        Guid id = this.roundService.StartNewRound(this.rooms.GetItem(roomId).Users, deck, roundTime, title, roomId);
        this.context?.Clients.Group(this.GetGroupKey(roomId))?.SendAsync("onRoundStarted", id.ToString()).Wait();
        return id;
      }

      return default;
    }

    /// <summary>
    /// Disconnect user from room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="user">User.</param>
    public void LeaveUser(Guid roomId, User user)
    {
      if (this.rooms.GetItem(roomId) != null)
      {
        this.context.Groups.RemoveFromGroupAsync(user?.ConnectionId, this.GetGroupKey(this.rooms.GetItem(roomId).Id)).Wait();
        this.context.Clients.Group(this.GetGroupKey(roomId)).SendAsync("onUserDisconnected", user, roomId).Wait();
        this.context.Clients.Client(user?.ConnectionId).SendAsync("onDisconnected").Wait();
        this.rooms.GetItem(roomId)?.Users?.Remove(user);
      }
    }

    /// <summary>
    /// Connect user to room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="newUser">User.</param>
    /// <param name="password">Password.</param>
    /// <returns>Succesfullness.</returns>
    public bool EnterUser(Guid roomId, User newUser, string password)
    {
      if ((this.rooms.GetItem(roomId)?.Password == password) && ((!this.rooms.GetItem(roomId)?.Users?.Contains(newUser)) ?? false))
      {
        this.userService.AddNewUser(newUser);
        this.rooms.GetItem(roomId)?.Users?.Add(newUser);

        this.context.Clients.Group(this.GetGroupKey(roomId))?.SendAsync("onUserConnected", newUser, this.rooms.GetItem(roomId).Users, roomId).Wait();
        this.context.Clients.Client(newUser.ConnectionId)?.SendAsync("onConnected", this.rooms.GetItem(roomId)).Wait();
        this.context.Groups.AddToGroupAsync(newUser?.ConnectionId, this.GetGroupKey(roomId)).Wait();
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Creating new room.
    /// </summary>
    /// <param name="host">Host.</param>
    /// <param name="name">Room name.</param>
    /// <param name="password">Room password.</param>
    /// <param name="cardInterpretation">Room card's interpretation.</param>
    /// <returns>Room ID.</returns>
    public Guid HostRoom(User host, string name = "Default name", string password = "", string cardInterpretation = "Hours")
    {
      var id = this.rooms.Add(new Room(host, name, password, cardInterpretation));
      this.isUsersReady.Add(new UsersReadiness(id));
      this.context.Groups.AddToGroupAsync(host?.ConnectionId, this.GetGroupKey(id)).Wait();
      return id;
    }

    /// <summary>
    /// Declares user readiness.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="user">User.</param>
    public void DeclareReady(Guid roomId, User user)
    {
      if ((this.isUsersReady.GetItemByRoomId(roomId) != null) && (user != null))
      {
        if (this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady?.ContainsKey(user) ?? false)
        {
          this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady[user] = true;
        }
        else
        {
          this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady?.Add(user, true);
        }

        this.context.Clients.Group(this.GetGroupKey(roomId)).SendAsync("onUserReady", user.ConnectionId, user.Name).Wait();

        if (this.IsUsersReady(roomId) && (this.rooms.GetItem(roomId) != null))
        {
          this.StartNewRound(roomId, this.rooms.GetItem(roomId).Host.Id, deckService.DefaultDeck);
          this.TurnReadinessToFalse(roomId);
        }
      }
    }

    /// <summary>
    /// Declares user not readiness.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="user">User.</param>
    public void DeclareNotReady(Guid roomId, User user)
    {
      if (this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady?.ContainsKey(user) ?? false)
      {
        this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady[user] = false;
      }
      else
      {
        this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady.Add(user, false);
      }

      this.context.Clients.Group(this.GetGroupKey(roomId)).SendAsync("onUserNotReady", user.ConnectionId, user.Name).Wait();
    }

    /// <summary>
    /// Determines readiness of all users in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Readiness of all users in room.</returns>
    public bool IsUsersReady(Guid roomId)
    {
      if (this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady?.Count != this.rooms.GetItem(roomId)?.Users?.Count())
      {
        return false;
      }

      bool flag = false;
      if (this.isUsersReady.GetItemByRoomId(roomId) != null)
      {
        foreach (var item in this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady)
        {
          flag = item.Value || flag;
        }
      }

      return flag;
    }

    /// <summary>
    /// Sets new card's interpretation.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="cardInterpretation">New card's interpretation.</param>
    public void ChangeCardInterpretation(Guid roomId, Guid userId, string cardInterpretation)
    {
      if (this.IsHost(userId, roomId) && (this.rooms.GetItem(roomId) != null))
      {
        this.rooms.GetItem(roomId).CardInterpretation = cardInterpretation;
      }
    }

    /// <summary>
    /// Sets new room name.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="name">New name.</param>
    public void ChangeRoomName(Guid roomId, Guid userId, string name)
    {
      if (this.IsHost(userId, roomId) && (this.rooms.GetItem(roomId) != null))
      {
        this.rooms.GetItem(roomId).Name = name;
      }
    }

    /// <summary>
    /// Sets new room host.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="newHostId">New host ID.</param>
    public void ChangeHost(Guid roomId, Guid userId, Guid newHostId)
    {
      if (this.IsHost(userId, roomId) && (this.rooms.GetItem(roomId) != null))
      {
        this.rooms.GetItem(roomId).Host = this.userService.GetUser(newHostId);
      }
    }

    /// <summary>
    /// Sets new password.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="newPassword">New password.</param>
    public void ChangePassword(Guid roomId, Guid userId, string newPassword)
    {
      if (this.IsHost(userId, roomId) && (this.rooms.GetItem(roomId) != null))
      {
        this.rooms.GetItem(roomId).Password = newPassword;
      }
    }

    /// <summary>
    /// Makes user's choice.
    /// </summary>
    /// <param name="roundId">Round ID.</param>
    /// <param name="user">User.</param>
    /// <param name="card">Card.</param>
    public void Choose(Guid roundId, User user, Card card)
    {
      this.roundService.UserChosed(this.rounds.GetItem(roundId), user, card);
    }

    /// <summary>
    /// Ends round.
    /// </summary>
    /// <param name="roundId">Round ID.</param>
    /// <param name="userId">User ID.</param>
    public void EndRound(Guid roundId, Guid userId)
    {
      if (this.IsHost(userId, this.rounds.GetItem(roundId).RoomId) && (this.rounds.GetItem(roundId) != null))
      {
        if (this.timers.GetTimer(roundId) != null)
        {
          this.timers.GetTimer(roundId)?.Stop();
        }
        else
        {
          this.rounds.GetItem(roundId).Duration = DateTime.Now - this.rounds.GetItem(roundId).StartDate;
        }

        this.context.Clients.Group(this.GetGroupKey(this.rounds.GetItem(roundId).RoomId))?.SendAsync("onEnd", new RoundDTO(this.rounds.GetItem(roundId))).Wait();
      }
    }

    /// <summary>
    /// Turn to false all readiness in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    private void TurnReadinessToFalse(Guid roomId)
    {
      var dict = this.isUsersReady.GetList().FirstOrDefault(x => x.RoomId == roomId).IsUsersReady;
      foreach (var key in dict.Keys.Cast<User>().ToArray())
      {
        dict[key] = false;
      }
    }

    /// <summary>
    /// Check user on host rights in room.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="roomId">Room ID</param>
    /// <returns>Is user host or not.</returns>
    private bool IsHost(Guid userId, Guid roomId)
    {
        return userId == this.rooms.GetItem(roomId)?.Host?.Id;
    }

    /// <summary>
    /// Build group key for room.
    /// </summary>
    /// <param name="roomId">Room ID</param>
    /// <returns>Group key.</returns>
    private string GetGroupKey(Guid roomId)
    {
      return $"room{roomId}";
    }
  }
}