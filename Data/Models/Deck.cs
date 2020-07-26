using System.Collections.Generic;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="Deck"/> class. Contains collection of not equal cards.
  /// </summary>
  public class Deck : IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Deck"/> class.
    /// </summary>
    /// <param name="cards">Collection of <see cref="Card"/>.</param>
    /// <param name="id">Deck id.</param>
    /// <param name="name">Deck name.</param>
    public Deck(List<Card> cards, string id, string name = "New deck")
    {
      this.Cards = cards.ToList();
      this.Name = name;
      this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Deck"/> class.
    /// </summary>
    /// <param name="id">Deck id.</param>
    /// <param name="name">Deck name.</param>
    public Deck(string id, string name = "New deck")
    {
      this.Cards = new List<Card>();
      this.Name = name;
      this.Id = id;
    }

    /// <summary>
    /// Gets or sets name of the deck.
    /// </summary>
    public string Name { get; set; } = "New deck";

    /// <summary>
    /// Gets ID of the deck.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets collection of cards in deck.
    /// </summary>
    public ICollection<Card> Cards { get; }

    /// <summary>
    /// Adding card in deck.
    /// </summary>
    /// <param name="newCard">New card.</param>
    public void AddCard(Card newCard)
    {
      if (!(this.Cards.Where(x => x.Name == newCard.Name).Count() > 0))
      {
        this.Cards?.Add(newCard);
      }
    }

    /// <summary>
    /// Removing card from deck.
    /// </summary>
    /// <param name="card">Card to remove</param>
    public void RemoveCard(Card card)
    {
      if (card != null && this.Cards.Contains(card))
      {
        this.Cards?.Remove(card);
      }
    }
  }
}