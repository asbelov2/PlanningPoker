using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace RoomApi
{
  /// <summary>
  /// RoundService class. Contains methods for round logic.
  /// </summary>
  public class RoundService
  {
    private RoundRepository rounds;
    private RoomRepository rooms;
    private RoundTimerService timers;
    private IHubContext<RoomHub> context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoundService"/> class.
    /// </summary>
    /// <param name="hubContext">Hub context.</param>
    /// <param name="roomRepository">Room repository.</param>
    /// <param name="roundRepository">Round repository.</param>
    /// <param name="roundTimerRepository">Round timer repository.</param>
    /// <param name="roundResultRepository">Round result repository.</param>
    public RoundService(
      IHubContext<RoomHub> hubContext,
      RoomRepository roomRepository,
      RoundRepository roundRepository,
      RoundTimerService roundTimerService)
    {
      this.context = hubContext;
      this.rooms = roomRepository;
      this.rounds = roundRepository;
      this.timers = roundTimerService;
    }

    /// <summary>
    /// Starting new round.
    /// </summary>
    /// <param name="users">Collection of users.</param>
    /// <param name="deck">Deck.</param>
    /// <param name="roundTime">Round time.</param>
    /// <param name="title">Round title.</param>
    /// <param name="roomId">RoomID.</param>
    /// <returns>This round ID.</returns>
    public Guid StartNewRound(ICollection<User> users, Deck deck, TimeSpan roundTime, string title, Guid roomId)
    {
      Guid id = this.rounds.Add(new Round(roomId, users, deck, roundTime, title));
      if (roundTime != TimeSpan.Zero)
      {
        this.timers.Add(new RoundTimer(id, roundTime));
        this.timers.GetTimer(id)?.SetTimer();
      }

      return id;
    }

    /// <summary>
    /// Makes user's choice.
    /// </summary>
    /// <param name="round">Round.</param>
    /// <param name="user">User.</param>
    /// <param name="card">Card.</param>
    public void UserChosed(Round round, User user, Card card)
    {
      if ((user != null) && (card != null) && (round != null) && (this.timers.GetTimer(round.Id)?.IsEnabled ?? true))
      {
        if (round.Deck.Cards.Contains(card))
        {
          if (round.Choices.Any(x => x.User == user))
          {
            round.Choices.FirstOrDefault(x => x.User == user).Card = card;
            this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onUserChosed", user).Wait();
            if (round.Choices.Count() == round.Users.Count())
            {
              this.timers.GetTimer(round.Id)?.Stop();
              this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onAllChosed", new RoundDTO(this.rounds.GetItem(round.Id))).Wait();
            }
          }
          else
          {
            round.Choices.Add(new Choice(user, card));
            this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onUserChosed", user).Wait();
            if (round.Choices.Count() == round.Users.Count())
            {
              if (this.timers.GetTimer(round.Id) == null)
              {
                round.Duration = DateTime.Now - round.StartDate;
              }

              this.timers.GetTimer(round.Id)?.Stop();
              this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onAllChosed", new RoundDTO(this.rounds.GetItem(round.Id))).Wait();
            }
          }
        }
        else
        {
          this.context.Clients.Client(user.ConnectionId).SendAsync("onWrongCard").Wait();
        }
      }
    }

    public IEnumerable<RoundDTO> GetRoundResultsByRoomId(Guid roomId)
    {
      var results = this.rounds.GetRoomRoundResults(roomId);
      var resultsDTOList = new List<RoundDTO>();
      foreach (var result in results)
      {
        resultsDTOList.Add(new RoundDTO(result));
      }
      return resultsDTOList;
    }

    /// <summary>
    /// Resets round timer.
    /// </summary>
    /// <param name="roundId">Round ID.</param>
    /// <param name="userId">User ID.</param>
    public void ResetTimer(Guid roundId, Guid userId)
    {
      if (this.IsHost(userId, this.rounds.GetItem(roundId)?.RoomId ?? default))
      {
        this.timers.GetTimer(roundId)?.SetTimer();
      }
    }

    /// <summary>
    /// Sets new round title.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="roundId">Round ID.</param>
    /// <param name="title">New title.</param>
    public void SetRoundTitle(Guid userId, Guid roundId, string title)
    {
      if (this.IsHost(userId, this.rounds.GetItem(roundId).RoomId) && (this.rounds.GetItem(roundId) != null))
      {
        this.rounds.GetItem(roundId).Title = title;
        this.context.Clients.Group(this.GetGroupKey(this.rounds.GetItem(roundId).RoomId))?.SendAsync("onRoundChanged", new RoundDTO(this.rounds.GetItem(roundId))).Wait();
      }
    }

    /// <summary>
    /// Sets round comment.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="roundId">Round ID.</param>
    /// <param name="comment">Comment.</param>
    public void SetRoundComment(Guid userId, Guid roundId, string comment)
    {
      if (this.IsHost(userId, this.rounds.GetItem(roundId).RoomId) && (this.rounds.GetItem(roundId) != null))
      {
        this.rounds.GetItem(roundId).Comment = comment;
        this.context.Clients.Group(this.GetGroupKey(this.rounds.GetItem(roundId).RoomId))?.SendAsync("onRoundChanged", new RoundDTO(this.rounds.GetItem(roundId))).Wait();
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
      return userId == this.rooms.GetItem(roomId).Host.Id;
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