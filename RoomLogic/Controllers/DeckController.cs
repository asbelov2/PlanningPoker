using System.Collections.Generic;
using System.Linq;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace RoomApi.Controllers
{
  /// <summary>
  /// Deck controller allows work with decks.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class DeckController : ControllerBase
  {
    private DeckService deckService;
    private DeckRepository decks;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeckController"/> class.
    /// </summary>
    /// <param name="deckService">Deck service.</param>
    /// <param name="deckRepository">Deck repository.</param>
    public DeckController(DeckService deckService, DeckRepository deckRepository)
    {
      this.deckService = deckService;
      this.decks = deckRepository;
    }

    /// <summary>
    /// Get all decks.
    /// </summary>
    /// <returns>All decks.</returns>
    [HttpGet]
    public ICollection<DeckDTO> Get()
    {
      var result = new List<DeckDTO>();
      foreach (var deck in this.decks.GetList())
      {
        if (deck != null)
        {
          result.Add(new DeckDTO(deck));
        }
      }

      return result;
    }

    /// <summary>
    /// Get deck by id.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    /// <returns>Deck.</returns>
    [HttpGet("{id}")]
    public DeckDTO Get(string id)
    {
      if (this.deckService.GetDeck(id) != null)
      {
        return new DeckDTO(this.deckService.GetDeck(id));
      }

      return null;
    }

    /// <summary>
    /// Creating new deck.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    /// <param name="name">Deck name.</param>
    [HttpPost]
    public void NewDeck(string id, string name)
    {
      this.deckService.NewDeck(id, name);
    }

    /// <summary>
    /// Add valuable card in deck.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    /// <param name="value">Card value.</param>
    [HttpPost("{id}/AddValuableCard")]
    public void AddValuableCard(string id, double value)
    {
      this.deckService.AddCard(this.decks.GetItem(id), new Card(CardType.Valuable, value.ToString(), value));
    }

    /// <summary>
    /// Add exceptional card in deck.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    /// <param name="name">Card name.</param>
    [HttpPost("{id}/AddExceptionalCard")]
    public void AddExceptionalCard(string id, string name)
    {
      this.deckService.AddCard(this.decks.GetItem(id), new Card(CardType.Exceptional, name, 0));
    }

    /// <summary>
    /// Delete card from deck.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    /// <param name="name">Card name.</param>
    [HttpDelete("{id}/DeleteCard")]
    public void DeleteCard(string id, string name)
    {
      this.deckService.RemoveCard(this.decks.GetItem(id), this.decks.GetItem(id).Cards.FirstOrDefault(x => x.Name == name));
    }

    /// <summary>
    /// Delete deck.
    /// </summary>
    /// <param name="id">Deck ID.</param>
    [HttpDelete("{id}")]
    public void DeleteDeck(string id)
    {
      this.deckService.DeleteDeck(id);
    }
  }
}