using Data;

namespace RoomApi
{
  /// <summary>
  /// Card data transfer object class. Used for transfering objects to client.
  /// </summary>
  public class CardDTO
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CardDTO"/> class.
    /// </summary>
    /// <param name="cardType">Type of card.</param>
    /// <param name="name">Name of card.</param>
    /// <param name="value">Value of card.</param>
    public CardDTO(CardType cardType, string name = "", double? value = 0)
    {
      this.Name = name;
      this.Value = value;
      this.CardType = cardType.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CardDTO"/> class.
    /// </summary>
    /// <param name="card">Card.</param>
    public CardDTO(Card card)
    {
      this.Name = card.Name;
      this.Value = card.Value;
      this.CardType = card.CardType.ToString();
    }

    /// <summary>
    /// Gets name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets value.
    /// </summary>
    public double? Value { get; }

    /// <summary>
    /// Gets type of card.
    /// </summary>
    public string CardType { get; }
  }
}