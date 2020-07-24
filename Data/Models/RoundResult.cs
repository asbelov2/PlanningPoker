using System;
using System.Collections.Generic;

namespace Data
{
  /// <summary>
  /// <see cref="RoundResult"/> class.
  /// </summary>
  public class RoundResult : IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RoundResult"/> class.
    /// </summary>
    /// <param name="round">Round.</param>
    public RoundResult(Round round)
    {
      this.Id = Guid.NewGuid().ToString();
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
    /// Gets RoundResult ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets room ID.
    /// </summary>
    public string RoomId { get; }

    /// <summary>
    /// Gets round ID.
    /// </summary>
    public string RoundId { get; }

    /// <summary>
    /// Gets choices.
    /// </summary>
    public List<Choice> Choices { get; }

    /// <summary>
    /// Gets resuilt.
    /// </summary>
    public double Result { get; }

    /// <summary>
    /// Gets date of start.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Gets start date.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Gets round title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets round comment.
    /// </summary>
    public string Comment { get; }
  }
}
