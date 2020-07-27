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
  /// <see cref="Card"/> class. Cards have name and value.
  /// </summary>
  /// <remarks>Value of exceptional cards = 0, Name of valuable card same as value.</remarks>
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
        this.Value = value;
      }
      else
      {
        this.Name = name;
        this.Value = null;
      }

      this.CardType = cardType;
    }

    /// <summary>
    /// Type of card.
    /// </summary>
    public CardType CardType { get; }

    /// <summary>
    /// Name of card.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Value of card.
    /// </summary>
    public double? Value { get; }
  }
}