using System;
using System.Linq;
using Data;
using Microsoft.AspNetCore.SignalR;
using NUnit.Framework;
using RoomApi;

namespace PlanningPokerTests
{
  internal class RepositoryTests
  {
    private DeckRepository decks;
    private RoomRepository rooms;
    private RoundRepository rounds;
    private UserRepository users;
    private RoundTimerService timers;
    private UsersReadinessService readiness;

    private IHubContext<RoomHub> context;
    private UserService userService;
    private RoomService roomService;
    private RoundService roundService;

    [SetUp]
    public void Setup()
    {
      this.decks = new DeckRepository();
      this.rooms = new RoomRepository();
      this.rounds = new RoundRepository();
      this.users = new UserRepository();
      this.timers = new RoundTimerService();
      this.readiness = new UsersReadinessService();

      this.context = HubContextImplementation.GetContext;
      this.userService = new UserService(this.users);
      this.roundService = new RoundService(this.context, rooms, rounds, timers);
      this.roomService = new RoomService(
        this.context,
        this.roundService,
        this.userService,
        new DeckService(this.decks),
        this.rooms,
        this.rounds,
        this.timers,
        this.readiness);
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
    public void AddAndDeleteAndGetList()
    {
      Assert.AreEqual(0, this.decks.GetList().Count());
      Deck deck = new Deck("111");
      this.decks.Add(deck);
      Assert.AreEqual(1, this.decks.GetList().Count());
      this.decks.Delete(deck);
      Assert.AreEqual(0, this.decks.GetList().Count());
    }

    [Test]
    public void GetItem()
    {
      Deck deck = new Deck("111");
      this.decks.Add(deck);
      Assert.AreEqual(deck, this.decks.GetItem(deck.Id));
    }

    [Test]
    public void WrongId()
    {
      Deck deck = new Deck("111");
      this.decks.Add(deck);
      Assert.IsNull(this.decks.GetItem(Guid.NewGuid()));
    }

    [Test]
    public void GetRoomRoundResults()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      var roomId = this.roomService.HostRoom(host);
      this.roomService.EnterUser(roomId, user1, string.Empty);

      var roundId = this.roomService.StartNewRound(roomId, host.Id, this.decks.GetItem(this.DefaultDeck()));
      this.roomService.EndRound(roundId, host.Id);

      roundId = this.roomService.StartNewRound(roomId, host.Id, this.decks.GetItem(this.DefaultDeck()));
      this.roomService.EndRound(roundId, host.Id);

      Assert.AreEqual(2, this.roundService.GetRoundResultsByRoomId(roomId).Count());

      roomId = this.roomService.HostRoom(user1);
      this.roomService.EnterUser(roomId, host, string.Empty);

      roundId = this.roomService.StartNewRound(roomId, user1.Id, this.decks.GetItem(this.DefaultDeck()));
      this.roomService.EndRound(roundId, user1.Id);

      Assert.AreEqual(3, this.rounds.GetList().Count());

    }

    private Guid DefaultDeck()
    {
      var defaultDeck = new Deck();
      double[] numbers = { 0, 1 / 2, 1, 2, 3, 5, 8, 13, 20, 40, 100 };
      foreach (var number in numbers)
      {
        defaultDeck.AddCard(new Card(CardType.Valuable, number.ToString(), number));
      }

      defaultDeck.AddCard(new Card(CardType.Exceptional, "?", 0));
      defaultDeck.AddCard(new Card(CardType.Exceptional, "∞", 0));
      defaultDeck.AddCard(new Card(CardType.Exceptional, "☕", 0));
      var decks = new DeckRepository();
      return decks.Add(defaultDeck);
    }
  }
}