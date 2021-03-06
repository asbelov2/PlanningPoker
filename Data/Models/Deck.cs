﻿using System;
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
    public Deck(List<Card> cards, string name = "New deck")
    {
      this.Cards = cards.ToList();
      this.Name = name;
      this.Id = Guid.NewGuid();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Deck"/> class.
    /// </summary>
    /// <param name="id">Deck id.</param>
    /// <param name="name">Deck name.</param>
    public Deck(string name = "New deck")
    {
      this.Cards = new List<Card>();
      this.Name = name;
      this.Id = Guid.NewGuid();
    }

    /// <summary>
    /// Name of the deck.
    /// </summary>
    public string Name { get; set; } = "New deck";

    /// <summary>
    /// ID of the deck.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Collection of cards in deck.
    /// </summary>
    public ICollection<Card> Cards { get; }

    /// <summary>
    /// Adding card in deck.
    /// </summary>
    /// <param name="newCard">New card.</param>
    public void AddCard(Card newCard)
    {
      if (!this.Cards.Any(x => x.Name == newCard.Name))
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