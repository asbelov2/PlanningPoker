﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
  /// <summary>
  /// <see cref="Round"/> class. Round must contain round's ID, room's ID, prepared deck.
  /// </summary>
  public class Round : IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Round"/> class.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="roomId">Room ID.</param>
    /// <param name="users">Collection of participants.</param>
    /// <param name="deck">Round deck.</param>
    /// <param name="roundTime">Round time.</param>
    /// <param name="title">Title of round.</param>
    public Round(Guid roomId, IEnumerable<User> users, Deck deck, TimeSpan roundTime, string title = "")
    {
      this.RoomId = roomId;
      this.Title = title;
      this.RoundTime = roundTime;
      this.Users = users;
      this.Id = Guid.NewGuid();
      this.Deck = deck;
      this.StartDate = DateTime.Now;
    }

    /// <summary>
    /// Gets room ID.
    /// </summary>
    public Guid RoomId { get; }

    /// <summary>
    /// Gets choices.
    /// </summary>
    public ICollection<Choice> Choices { get; } = new List<Choice>();

    /// <summary>
    /// Gets or sets comment.
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Gets date of round start.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Gets or sets duration of round.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets deck
    /// </summary>
    public Deck Deck { get; }

    /// <summary>
    /// Gets round ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets round result.
    /// </summary>
    public double? Result
    {
      get
      {
        if (this.Choices.Count <= 0)
        {
          return null;
        }

        double result = 0;
        foreach (var choice in this.Choices)
        {
          if (choice.Card.CardType == CardType.Exceptional)
          {
            return null;
          }

          result += choice.Card.Value;
        }

        return result / this.Choices.Count();
      }
    }

    /// <summary>
    /// Gets round time.
    /// </summary>
    public TimeSpan RoundTime { get; }

    /// <summary>
    /// Gets or sets round title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets collection of participants.
    /// </summary>
    public IEnumerable<User> Users { get; }
  }
}