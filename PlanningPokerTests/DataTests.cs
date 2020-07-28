using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Microsoft.AspNetCore.SignalR;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RoomApi;

namespace PlanningPokerTests
{
  public class DataTests
  {
    private IHubContext<RoomHub> context;
    private RoundTimerService timers;
    private DeckRepository decks;
    private DeckService deckService;
    private RoomRepository rooms;
    private RoundRepository rounds;
    private RoundService roundService;
    private UserRepository users;

    [SetUp]
    public void Setup()
    {
      this.context = HubContextImplementation.GetContext;
      this.timers = new RoundTimerService();
      this.decks = new DeckRepository();
      this.rooms = new RoomRepository();
      this.rounds = new RoundRepository();
      this.users = new UserRepository();
      this.deckService = new DeckService(decks);
      this.roundService = new RoundService(this.context, this.rooms, this.rounds, this.timers);
      InitDefaultDeck();
    }

    [TearDown]
    public void TearDown()
    {
      this.decks.ClearRepository();
      this.rooms.ClearRepository();
      this.rounds.ClearRepository();
      this.users.ClearRepository();
    }

    [Test]
    public void CreateCard()
    {
      Card card = new Card(CardType.Exceptional, "test", 111);
      Assert.IsNull(card.Value);
      Assert.AreEqual(card.Name, "test");
      card = new Card(CardType.Valuable, "1", 2);
      Assert.AreEqual(card.Value, 2);
      Assert.AreEqual(card.Name, "2");
    }

    [Test]
    public void CreateDeck()
    {
      Deck testDeck = new Deck("testDeck");
      Assert.AreEqual(0, testDeck.Cards.Count);
      Assert.AreEqual("testDeck", testDeck.Name);
    }

    [Test]
    public void AddCardInDeck()
    {
      Deck testDeck = new Deck("testDeck");
      testDeck.AddCard(new Card(CardType.Valuable, value: 1));
      testDeck.AddCard(new Card(CardType.Exceptional, name: "test"));
      Assert.AreEqual(testDeck.Cards.Count, 2);
    }

    [Test]
    public void RemoveCardFromDeck()
    {
      Deck testDeck = new Deck("testDeck");
      testDeck.AddCard(new Card(CardType.Valuable, value: 1));
      testDeck.AddCard(new Card(CardType.Exceptional, name: "test"));
      testDeck.RemoveCard(testDeck.Cards.ElementAt(0));
      Assert.AreEqual(testDeck.Cards.Count, 1);
    }

    [Test]
    public void AllEmptyParameters()
    {
      User host = new User("Host", "1");
      Room testRoom = new Room(host, string.Empty, string.Empty, string.Empty);
      Assert.AreEqual(testRoom.Name, string.Empty);
      Assert.AreEqual(testRoom.Password, string.Empty);
      Assert.AreEqual(testRoom.CardInterpretation, string.Empty);
    }

    public void HostNotNull()
    {
      User host = new User("Host", "1");
      Room testRoom = new Room(host, string.Empty, string.Empty, string.Empty);

      Assert.NotNull(testRoom.Host);
    }

    [Test]
    public void CorrectResult()
    {
      var users = new List<User>();
      users.Add(new User("1", "1"));
      users.Add(new User("2", "2"));
      Round testRound = new Round(Guid.NewGuid(), users, deckService.DefaultDeck, TimeSpan.FromMinutes(5), "test");
      testRound.Choices.Add(new Choice(users[0], testRound.Deck.Cards.FirstOrDefault(x => x.Name == "2")));
      testRound.Choices.Add(new Choice(users[1], testRound.Deck.Cards.FirstOrDefault(x => x.Name == "5")));
      Assert.AreEqual(3.5, testRound.Result);
    }

    [Test]
    public void CorrectUserCount()
    {
      var users = new List<User>();
      users.Add(new User("1", "1"));
      users.Add(new User("2", "2"));
      Round testRound = new Round(Guid.NewGuid(), users, deckService.DefaultDeck, TimeSpan.FromMinutes(5), "test");
      Assert.AreEqual(testRound.Users.Count(), 2);
    }

    [Test]
    public void CorrectUserId()
    {
      User testUser = new User("Vasya", "1");
      Assert.AreEqual(testUser.ConnectionId, "1");
    }

    [Test]
    public void CorrectUserName()
    {
      User testUser = new User("Vasya", "1");
      Assert.AreEqual(testUser.Name, "Vasya");
    }

    [Test]
    public void RoundTimerCorrectWork()
    {
      var users = new List<User>();
      users.Add(new User("1", "1"));
      users.Add(new User("2", "2"));
      Round testRound = new Round(Guid.NewGuid(), users, deckService.DefaultDeck, TimeSpan.FromMinutes(5), "Test");
      this.rounds.Add(testRound);

      RoundTimer timer = new RoundTimer(testRound.Id, TimeSpan.FromMinutes(5), this.roundService);
      timer.SetTimer();
      timer.Stop();
      Assert.IsNotNull(this.rounds.GetList().First().Duration);

      Guid testId = Guid.NewGuid();
      testRound = new Round(testId, users, deckService.DefaultDeck, TimeSpan.FromMinutes(5), "Test2");
      this.rounds.Add(testRound);
      timer = new RoundTimer(testRound.Id, TimeSpan.FromMinutes(5), this.roundService);
      timer.SetTimer();
      timer.Stop();
      Assert.IsNotNull(this.rounds.GetList().Last().Duration);
    }

    private void InitDefaultDeck()
    {
      var defaultDeck = new Deck("DefaultDeck");
      double[] numbers = { 0, 1.0 / 2, 1, 2, 3, 5, 8, 13, 20, 40, 100 };
      foreach (var number in numbers)
      {
        defaultDeck.AddCard(new Card(CardType.Valuable, number.ToString(), number));
      }

      defaultDeck.AddCard(new Card(CardType.Exceptional, "?", 0));
      defaultDeck.AddCard(new Card(CardType.Exceptional, "∞", 0));
      defaultDeck.AddCard(new Card(CardType.Exceptional, "☕", 0));
      this.deckService.DefaultDeck = decks.GetItem(this.decks.Add(defaultDeck));
    }
  }
}