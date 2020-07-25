using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.SignalR;

namespace RoomApi
{
  /// <summary>
  /// RoomService class.
  /// </summary>
  public class RoomService
  {
    private IHubContext<RoomHub> context;
    private UserService userService = new UserService();
    private RoomRepository rooms = new RoomRepository();
    private RoundService roundService;
    private RoundRepository rounds = new RoundRepository();
    private UsersReadinessRepository isUsersReady = new UsersReadinessRepository();
    private RoundTimerRepository timers = new RoundTimerRepository();

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomService"/> class.
    /// </summary>
    /// <param name="hubContext">Hub context.</param>
    public RoomService(IHubContext<RoomHub> hubContext)
    {
      this.context = hubContext;
      this.roundService = new RoundService(hubContext);
    }

    /// <summary>
    /// Starts new round.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="title">Title.</param>
    /// <param name="deck">Deck.</param>
    /// <param name="roundTime">Round time.</param>
    /// <returns>Round ID.</returns>
    public string StartNewRound(string roomId, string userId, string title = "Default title", Deck deck = null, TimeSpan roundTime = default)
    {
      if (this.IsHost(userId, roomId))
      {
        string id = Guid.NewGuid().ToString();
        this.context?.Clients.Group(this.GetGroupKey(roomId))?.SendAsync("onRoundStarted", id).Wait();
        return this.roundService.StartNewRound(this.rooms.GetItem(roomId).Users, deck ?? new DefaultDeck(), roundTime, id, title, roomId);
      }

      return string.Empty;
    }

    /// <summary>
    /// Disconnect user from room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="user">User.</param>
    public void LeaveUser(string roomId, User user)
    {
      this.context.Groups.RemoveFromGroupAsync(user?.ConnectionId, this.GetGroupKey(this.rooms.GetItem(roomId)?.Id)).Wait();
      this.context.Clients.Group(this.GetGroupKey(roomId)).SendAsync("onUserDisconnected", user, roomId).Wait();
      this.context.Clients.Client(user?.Id).SendAsync("onDisconnected").Wait();
      this.rooms.GetItem(roomId)?.Users?.Remove(user);
    }

    /// <summary>
    /// Connect user to room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="newUser">User.</param>
    /// <param name="password">Password.</param>
    /// <returns>Succesfullness.</returns>
    public bool EnterUser(string roomId, User newUser, string password)
    {
      if ((this.rooms.GetItem(roomId)?.Password == password) && ((!this.rooms.GetItem(roomId)?.Users?.Contains(newUser)) ?? false))
      {
        this.userService.AddNewUser(newUser);
        this.rooms.GetItem(roomId)?.Users?.Add(newUser);

        this.context.Clients.Group(this.GetGroupKey(roomId))?.SendAsync("onUserConnected", newUser, this.rooms.GetItem(roomId).Users, roomId).Wait();
        this.context.Clients.Client(newUser.Id)?.SendAsync("onConnected", this.rooms.GetItem(roomId)).Wait();
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
    /// <param name="id">Room ID.</param>
    /// <param name="host">Host.</param>
    /// <param name="name">Room name.</param>
    /// <param name="password">Room password.</param>
    /// <param name="cardInterpretation">Room card's interpretation.</param>
    public void HostRoom(string id, User host, string name = "Default name", string password = "", string cardInterpretation = "Hours")
    {
      this.isUsersReady.Add(new UsersReadiness(id));
      this.rooms.Add(new Room(id, host, name, password, cardInterpretation));
      this.context.Groups.AddToGroupAsync(host?.ConnectionId, this.GetGroupKey(id)).Wait();
    }

    /// <summary>
    /// Declares user readiness.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="user">User.</param>
    /// <returns>Async task.</returns>
    public async Task DeclareReady(string roomId, User user)
    {
      if (this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady?.ContainsKey(user) ?? false)
      {
        this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady[user] = true;
      }
      else
      {
        this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady.Add(user, true);
      }

      await this.context.Clients.Group(this.GetGroupKey(roomId)).SendAsync("onUserReady", user.Id, user.Name);

      if (this.IsUsersReady(roomId))
      {
        this.StartNewRound(roomId, this.rooms.GetItem(roomId)?.Host.Id);
        this.TurnToFalse(roomId);
      }
    }

    /// <summary>
    /// Declares user not readiness.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="user">User.</param>
    /// <returns>Async task.</returns>
    public async Task DeclareNotReady(string roomId, User user)
    {
      if (this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady?.ContainsKey(user) ?? false)
      {
        this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady[user] = false;
      }
      else
      {
        this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady.Add(user, false);
      }

      await this.context.Clients.Group(this.GetGroupKey(roomId)).SendAsync("onUserNotReady", user.Id, user.Name);
    }

    /// <summary>
    /// Determines readiness of all users in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Readiness of all users in room.</returns>
    public bool IsUsersReady(string roomId)
    {
      if (this.isUsersReady.GetItemByRoomId(roomId)?.IsUsersReady?.Count != this.rooms.GetItem(roomId)?.Users?.Count())
      {
        return false;
      }

      bool flag = false;
      foreach (var item in this.isUsersReady.GetItemByRoomId(roomId).IsUsersReady)
      {
        flag = item.Value || flag;
      }

      return flag;
    }

    /// <summary>
    /// Sets new card's interpretation.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="cardInterpretation">New card's interpretation.</param>
    public void ChangeCardInterpretation(string roomId, string userId, string cardInterpretation)
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
    public void ChangeRoomName(string roomId, string userId, string name)
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
    public void ChangeHost(string roomId, string userId, string newHostId)
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
    public void ChangePassword(string roomId, string userId, string newPassword)
    {
      if (this.IsHost(userId, roomId) && (this.rooms.GetItem(roomId) != null))
      {
        this.rooms.GetItem(roomId).Password = newPassword;
      }
    }

    /// <summary>
    /// Sets new round title.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="roundId">Round ID.</param>
    /// <param name="title">New title.</param>
    /// <returns>Async task.</returns>
    public async Task SetRoundTitle(string userId, string roundId, string title)
    {
      if (this.IsHost(userId, this.rounds.GetItem(roundId).RoomId) && (this.rounds.GetItem(roundId) != null))
      {
        this.roundService.SetTitle(this.rounds.GetItem(roundId), title);
        await this.context.Clients.Group(this.GetGroupKey(this.rounds.GetItem(roundId).RoomId))?.SendAsync("onRoundChanged", new RoundDTO(this.rounds.GetItem(roundId)));
      }
    }

    /// <summary>
    /// Sets round comment.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="roundId">Round ID.</param>
    /// <param name="comment">Comment.</param>
    /// <returns>Async task.</returns>
    public async Task SetRoundComment(string userId, string roundId, string comment)
    {
      if (this.IsHost(userId, this.rounds.GetItem(roundId).RoomId) && (this.rounds.GetItem(roundId) != null))
      {
        this.roundService.SetComment(this.rounds.GetItem(roundId), comment);
        await this.context.Clients.Group(this.GetGroupKey(this.rounds.GetItem(roundId).RoomId))?.SendAsync("onRoundChanged", new RoundDTO(this.rounds.GetItem(roundId)));
      }
    }

    /// <summary>
    /// Makes user's choice.
    /// </summary>
    /// <param name="roundId">Round ID.</param>
    /// <param name="user">User.</param>
    /// <param name="card">Card.</param>
    /// <returns>Async task.</returns>
    public async Task Choose(string roundId, User user, Card card)
    {
      await this.roundService.UserChosed(this.rounds.GetItem(roundId), user, card);
    }

    /// <summary>
    /// Ends round.
    /// </summary>
    /// <param name="roundId">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <returns>Async task.</returns>
    public async Task EndRound(string roundId, string userId)
    {
      if (this.IsHost(userId, this.rounds.GetItem(roundId).RoomId) && (this.rounds.GetItem(roundId) != null))
      {
        this.timers.GetItem(roundId).Stop();
        await this.context.Clients.Group(this.GetGroupKey(this.rounds.GetItem(roundId).RoomId))?.SendAsync("onEnd", new RoundDTO(this.rounds.GetItem(roundId)));
      }
    }


    /// <summary>
    /// Turn to false all readiness in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    private void TurnToFalse(string roomId)
    {
      var dict = this.isUsersReady.GetList().FirstOrDefault(x => x.RoomId == roomId).IsUsersReady;
      foreach (var key in dict.Keys.Cast<User>().ToArray())
      {
        dict[key] = false;
      }
    }

    private bool IsHost(string userId, string roomId)
    {
      return this.userService.GetUser(userId).Id == this.rooms.GetItem(roomId).Host.Id;
    }

    private string GetGroupKey(string roomId)
    {
      return $"room{roomId}";
    }
  }
}