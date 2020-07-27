using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="DeckRepository"/> class. Stores created decks.
  /// </summary>
  public class DeckRepository : Repository<Deck>
  {
    /// <summary>
    /// Gets default deck from deck.
    /// </summary>
    /// <returns>Deck.</returns>
    public Deck GetDefaultDeck()
    {
      return Data.FirstOrDefault(x => x.Name == "DefaultDeck");
    }
  }
}