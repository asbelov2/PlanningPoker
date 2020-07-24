namespace Data
{
  /// <summary>
  /// Type of card.
  /// </summary>
  public enum CardType
  {
    /// <summary>
    /// Card have numerical value
    /// </summary>
    Valuable,

    /// <summary>
    /// Card haven't numerical value
    /// </summary>
    Exceptional,
  }

  /// <summary>
  /// <see cref="Card"/> class.
  /// </summary>
  public class Card
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class.
    /// </summary>
    /// <param name="cardType">Type of card.</param>
    /// <param name="name">Name of card.</param>
    /// <param name="value">Value of card.</param>
    /// <remarks>Value of exceptional cards = 0, Name of valuable card same as value.</remarks>
    public Card(CardType cardType, string name = "", double value = 0)
    {
      if (cardType == CardType.Valuable)
      {
        this.Name = value.ToString();
      }
      else
      {
        this.Name = name;
      }

      if (cardType == CardType.Valuable)
      {
        this.Value = value;
      }
      else
      {
        this.Value = 0;
      }

      this.CardType = cardType;
    }

    /// <summary>
    /// Gets type of card.
    /// </summary>
    public CardType CardType { get; }

    /// <summary>
    /// Gets name of card.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets value of card.
    /// </summary>
    public double Value { get; }
  }
}