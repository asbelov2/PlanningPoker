namespace Data
{
  /// <summary>
  /// <see cref="DefaultDeck"/> class. Contains default cards: 0, 1/2, 1, 2, 3, 5, 8, 13, 20, 40, 100, ?, ∞, ☕.
  /// </summary>
  public class DefaultDeck : Deck
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultDeck"/> class.
    /// </summary>
    public DefaultDeck()
      : base()
    {
      double[] numbers = { 0, 1 / 2, 1, 2, 3, 5, 8, 13, 20, 40, 100 };
      foreach (var number in numbers)
      {
        this.AddCard(new Card(CardType.Valuable, number.ToString(), number));
      }

      this.AddCard(new Card(CardType.Exceptional, "?", 0));
      this.AddCard(new Card(CardType.Exceptional, "∞", 0));
      this.AddCard(new Card(CardType.Exceptional, "☕", 0));
    }
  }
}