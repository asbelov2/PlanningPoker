using System;
using System.Collections.Generic;
using Data;

namespace RoomApi
{
  /// <summary>
  /// RoundDTO class.
  /// </summary>
  public class RoundDTO
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RoundDTO"/> class.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="users">Collection of users.</param>
    /// <param name="deck">Deck.</param>
    /// <param name="roundTime">Round time.</param>
    /// <param name="title">Round title.</param>
    /// <param name="result">Round result.</param>
    /// <param name="comment">Round comment.</param>
    /// <param name="choices">Collection of choices.</param>
    /// <param name="duration">Round duration.</param>
    /// <param name="startDate">Date of start round.</param>
    public RoundDTO(
      string id,
      ICollection<UserDTO> users,
      DeckDTO deck,
      double roundTime,
      string title,
      double result,
      string comment,
      ICollection<ChoiceDTO> choices,
      TimeSpan duration,
      DateTime startDate)
    {
      this.Id = id;
      this.Users = users;
      this.Deck = deck;
      this.RoundTime = roundTime;
      this.Title = title;
      this.Result = result;
      this.Comment = comment;
      this.Choices = choices;
      this.Duration = duration;
      this.StartDate = startDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoundDTO"/> class.
    /// </summary>
    /// <param name="round">Round.</param>
    public RoundDTO(Round round)
    {
      this.Id = round.Id;
      this.Users = new List<UserDTO>();
      foreach (var user in round.Users)
      {
        this.Users.Add(new UserDTO(user));
      }

      this.Deck = new DeckDTO(round.Deck);
      this.RoundTime = round.RoundTime.TotalMinutes;
      this.Title = round.Title;
      this.Result = round.Result;
      this.Comment = round.Comment;
      this.Choices = new List<ChoiceDTO>();
      foreach (var choice in round.Choices)
      {
        this.Choices.Add(new ChoiceDTO(choice));
      }

      this.Duration = round.Duration;
      this.StartDate = round.StartDate;
    }

    /// <summary>
    /// Gets date of start round.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Gets duration.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Gets round time.
    /// </summary>
    public double RoundTime { get; }

    /// <summary>
    /// Gets result.
    /// </summary>
    public double Result { get; }

    /// <summary>
    /// Gets deck.
    /// </summary>
    public DeckDTO Deck { get; }

    /// <summary>
    /// Gets title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets comment.
    /// </summary>
    public string Comment { get; }

    /// <summary>
    /// Gets collection of users.
    /// </summary>
    public ICollection<UserDTO> Users { get; }

    /// <summary>
    /// Gets collection of choices.
    /// </summary>
    public ICollection<ChoiceDTO> Choices { get; }

    /// <summary>
    /// Round ID.
    /// </summary>
    public string Id { get; }
  }
}