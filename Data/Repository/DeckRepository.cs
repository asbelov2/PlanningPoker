using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="DeckRepository"/> class. Stores created decks.
  /// </summary>
  public class DeckRepository : Repository<Deck>
  {
    public DeckRepository(ApplicationContext context) : base(context)
    {

    }

    /// <summary>
    /// Gets default deck from deck.
    /// </summary>
    /// <returns>Deck.</returns>
    public Deck GetDefaultDeck()
    {
      return this.GetItems(x => x.Name == "DefaultDeck").FirstOrDefault();
    }
  }
}