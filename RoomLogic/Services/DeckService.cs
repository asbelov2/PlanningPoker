using System;
using System.Collections.Generic;
using Data;

namespace RoomApi
{
  /// <summary>
  /// DeckService class. Allows work with deck repository.
  /// </summary>
  public class DeckService
  {
    private static DeckRepository decks;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeckService"/> class.
    /// </summary>
    /// <param name="deckRepository">Deck repository.</param>
    public DeckService(DeckRepository deckRepository)
    {
      decks = deckRepository;
    }

    /// <summary>
    /// Create new deck in repository.
    /// </summary>
    /// <param name="name">Deck name.</param>
    public Guid NewDeck(string name = "New deck")
    {
      return decks.Add(new Deck(name));
    }

    /// <summary>
    /// Delete deck from repository.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    public void DeleteDeck(Guid id)
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
    /// Add card in deck.
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
    public Deck GetDeck(Guid id)
    {
      return decks.GetItem(id);
    }

    /// <summary>
    /// Get all deck from repository.
    /// </summary>
    /// <returns>Collection of decks.</returns>
    public IEnumerable<Deck> GetDecks()
    {
      return decks.GetList();
    }

    /// <summary>
    /// Get default deck from repository.
    /// </summary>
    public Deck DefaultDeck { get; set; }
  }
}