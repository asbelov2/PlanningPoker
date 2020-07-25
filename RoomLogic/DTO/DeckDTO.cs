using System.Collections.Generic;
using Data;

namespace RoomApi
{
  /// <summary>
  /// DeckDTO class.
  /// </summary>
  public class DeckDTO
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DeckDTO"/> class.
    /// </summary>
    /// <param name="cards">Cards collection.</param>
    /// <param name="id">Deck ID.</param>
    /// <param name="name">Deck name.</param>
    public DeckDTO(List<CardDTO> cards, string id, string name = "New deck")
    {
      this.Cards = cards;
      this.Id = id;
      this.Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeckDTO"/> class.
    /// </summary>
    /// <param name="deck">Deck.</param>
    public DeckDTO(Deck deck)
    {
      this.Cards = new List<CardDTO>();
      foreach (var card in deck.Cards)
      {
        this.Cards.Add(new CardDTO(card));
      }

      this.Id = deck.Id;
      this.Name = deck.Name;
    }

    /// <summary>
    /// Gets name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets collection of cards.
    /// </summary>
    public ICollection<CardDTO> Cards { get; }
  }
}