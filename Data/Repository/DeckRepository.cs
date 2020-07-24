namespace Data
{
  /// <summary>
  /// <see cref="DeckRepository"/> class.
  /// </summary>
  public class DeckRepository : Repository<Deck>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DeckRepository"/> class.
    /// </summary>
    public DeckRepository()
    {
      this.Add(new DefaultDeck());
    }
  }
}