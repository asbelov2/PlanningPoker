using System.Collections.Generic;
using Data;

namespace RoomApi
{
  /// <summary>
  /// DeckService class.
  /// </summary>
  public class DeckService
  {
    private static DeckRepository decks;

    /// <summary>
    /// Initializes static members of the <see cref="DeckService"/> class.
    /// </summary>
    static DeckService()
    {
      decks = new DeckRepository();
    }

    /// <summary>
    /// Create new deck in repository.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    /// <param name="name">Deck name.</param>
    public void NewDeck(string id, string name = "New deck")
    {
      decks.Add(new Deck(id, name));
    }

    /// <summary>
    /// Delete deck from repository.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    public void DeleteDeck(string id)
    {
      decks.Delete(decks.GetItem(id));
    }

    /// <summary>
    /// Delete deck from repository.
    /// </summary>
    /// <param name="deck">Deck.</param>
    public void DeleteDeck(Deck deck)
    {
      decks.Delete(deck);
    }

    /// <summary>
    /// Add card in deck
    /// </summary>
    /// <param name="deck">Deck.</param>
    /// <param name="card">Card.</param>
    public void AddCard(Deck deck, Card card)
    {
      deck.AddCard(card);
    }

    /// <summary>
    /// Remove card from deck.
    /// </summary>
    /// <param name="deck">Deck.</param>
    /// <param name="card">Card.</param>
    public void RemoveCard(Deck deck, Card card)
    {
      deck.RemoveCard(card);
    }

    /// <summary>
    /// Get deck from repository.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    /// <returns>Deck.</returns>
    public Deck GetDeck(string id)
    {
      return decks.GetItem(id);
    }

    /// <summary>
    /// Get all deck from repository.
    /// </summary>
    /// <returns>Collection of decks.</returns>
    public ICollection<Deck> GetDecks()
    {
      return decks.GetList();
    }
  }
}