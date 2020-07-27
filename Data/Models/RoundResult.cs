using System;
using System.Collections.Generic;

namespace Data
{
  /// <summary>
  /// <see cref="RoundResult"/> class. Contain information about round (usually after it's end).
  /// </summary>
  public class RoundResult : IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RoundResult"/> class.
    /// </summary>
    /// <param name="round">Round.</param>
    public RoundResult(Round round)
    {
      this.Id = Guid.NewGuid();
      this.RoomId = round.RoomId;
      this.RoundId = round.Id;
      this.Choices = round.Choices;
      this.Result = round.Result;
      this.StartDate = round.StartDate;
      this.Duration = round.Duration;
      this.Title = round.Title;
      this.Comment = round.Comment;
    }

    /// <summary>
    /// RoundResult ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Room ID.
    /// </summary>
    public Guid RoomId { get; }

    /// <summary>
    /// Round ID.
    /// </summary>
    public Guid RoundId { get; }

    /// <summary>
    /// Choices.
    /// </summary>
    public ICollection<Choice> Choices { get; }

    /// <summary>
    /// Resuilt.
    /// </summary>
    public double? Result { get; }

    /// <summary>
    /// Date of start.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Start date.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Round title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Round comment.
    /// </summary>
    public string Comment { get; }
  }
}
