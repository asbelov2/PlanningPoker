using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.SignalR;

namespace RoomApi
{
  /// <summary>
  /// RoundService class.
  /// </summary>
  public class RoundService
  {
    private static RoundRepository rounds = new RoundRepository();
    private RoomRepository rooms = new RoomRepository();
    private RoundTimerRepository timers = new RoundTimerRepository();
    private IHubContext<RoomHub> context;
    private RoundResultRepository roundResults = new RoundResultRepository();

    /// <summary>
    /// Initializes a new instance of the <see cref="RoundService"/> class.
    /// </summary>
    /// <param name="hubContext">Hub context.</param>
    public RoundService(IHubContext<RoomHub> hubContext)
    {
      this.context = hubContext;
    }

    /// <summary>
    /// Starting new round
    /// </summary>
    /// <param name="users">Collection of users.</param>
    /// <param name="deck">Deck.</param>
    /// <param name="roundTime">Round time.</param>
    /// <param name="id">Round ID.</param>
    /// <param name="title">Round title.</param>
    /// <param name="roomId">RoomID.</param>
    /// <returns>This round ID.</returns>
    public string StartNewRound(ICollection<User> users, Deck deck, TimeSpan roundTime, string id, string title, string roomId)
    {
      rounds.Add(new Round(id, roomId, users, deck, roundTime, title));
      if (roundTime != TimeSpan.Zero)
      {
        this.timers.Add(new RoundTimer(id, roundTime));
        this.timers.GetItem(id).SetTimer();
      }

      return id;
    }

    /// <summary>
    /// Resets round timer.
    /// </summary>
    /// <param name="roundId">Round ID.</param>
    /// <param name="userId">User ID.</param>
    public void ResetTimer(string roundId, string userId)
    {
      if (userId == this.rooms.GetItem(rounds.GetItem(roundId)?.RoomId)?.Host?.Id)
      {
        this.timers.GetItem(roundId)?.SetTimer();
      }
    }

    /// <summary>
    /// Makes user's choice.
    /// </summary>
    /// <param name="round">Round.</param>
    /// <param name="user">User.</param>
    /// <param name="card">Card.</param>
    /// <returns>Async task</returns>
    public async Task UserChosed(Round round, User user, Card card)
    {
      if (round != null && this.timers.GetItem(round.Id).IsEnabled)
      {
        if (round.Deck.Cards.Contains(card))
        {
          if (round.Choices.Select(x => x.User == user).Count() > 0)
          {
            round.Choices.FirstOrDefault(x => x.User == user).Card = card;
            await this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onUserChosed", user);
            if (round.Choices.Count() == round.Users.Count())
            {
              this.timers.GetItem(round.Id).Stop();
              await this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onAllChosed", new RoundDTO(rounds.GetItem(round.Id)));
            }
          }
          else
          {
            round.Choices.Add(new Choice(user, card));
            await this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onUserChosed", user);
            if (round.Choices.Count() == round.Users.Count())
            {
              this.roundResults.Add(new RoundResult(rounds.GetItem(round.Id)));
              this.timers.GetItem(round.Id).Stop();
              await this.context.Clients.Group(this.GetGroupKey(round.RoomId)).SendAsync("onAllChosed", new RoundDTO(rounds.GetItem(round.Id)));
            }
          }
        }
        else
        {
          await this.context.Clients.Client(user.ConnectionId).SendAsync("onWrongCard");
        }
      }
    }

    /// <summary>
    /// Sets round comment.
    /// </summary>
    /// <param name="round">Round.</param>
    /// <param name="comment">Comment.</param>
    public void SetComment(Round round, string comment)
    {
      if (round != null)
      {
        round.Comment = comment;
      }
    }

    /// <summary>
    /// Sets round title.
    /// </summary>
    /// <param name="round">Round.</param>
    /// <param name="title">Title.</param>
    public void SetTitle(Round round, string title)
    {
      if (round != null)
      {
        round.Title = title;
      }
    }

    private string GetGroupKey(string roomId)
    {
      return $"room{roomId}";
    }
  }
}