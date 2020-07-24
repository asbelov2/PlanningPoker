﻿namespace Data
{
  /// <summary>
  /// <see cref="Choice"/> class.
  /// </summary>
  public class Choice
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Choice"/> class.
    /// </summary>
    /// <param name="user">User that chosed.</param>
    /// <param name="card">Card that was chosed.</param>
    public Choice(User user, Card card)
    {
      this.User = user;
      this.Card = card;
    }

    /// <summary>
    /// Gets or sets user.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Gets or sets card.
    /// </summary>
    public Card Card { get; set; }
  }
}