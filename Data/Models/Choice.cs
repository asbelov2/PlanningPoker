using System;

namespace Data
{
  /// <summary>
  /// <see cref="Choice"/> class. Contatins choice of one participant.
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
      this.Id = Guid.NewGuid();
      this.User = user;
      this.Card = card;
    }

    public Choice()
    {
      this.Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Card.
    /// </summary>
    public Card Card { get; set; }
  }
}