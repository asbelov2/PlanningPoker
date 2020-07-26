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
    private RoundTimerRepository timers;
    private RoundResultRepository results;
    private UsersReadinessRepository readiness;

    private IHubContext<RoomHub> context;
    private UserService userService;
    private RoomService roomService;

    [SetUp]
    public void Setup()
    {
      this.decks = new DeckRepository();
      this.rooms = new RoomRepository();
      this.rounds = new RoundRepository();
      this.users = new UserRepository();
      this.timers = new RoundTimerRepository();
      this.results = new RoundResultRepository();
      this.readiness = new UsersReadinessRepository();

      this.context = HubContextImplementation.GetContext;
      this.userService = new UserService(this.users);
      this.roomService = new RoomService(
        this.context,
        new RoundService(this.context, this.rooms, this.rounds, this.timers, this.results),
        this.userService,
        this.rooms,
        this.rounds,
        this.timers,
        this.readiness,
        this.results);
    }

    [TearDown]
    public void TearDown()
    {
      this.decks.ClearRepository();
      this.rooms.ClearRepository();
      this.rounds.ClearRepository();
      this.users.ClearRepository();
      this.timers.ClearRepository();
      this.results.ClearRepository();
      this.readiness.ClearRepository();
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

      var roundId = this.roomService.StartNewRound(roomId, host.Id, new DefaultDeck());
      this.roomService.EndRound(roundId, host.Id).Wait();

      roundId = this.roomService.StartNewRound(roomId, host.Id, new DefaultDeck());
      this.roomService.EndRound(roundId, host.Id).Wait();

      Assert.AreEqual(2, this.results.GetRoomRoundResults(roomId).Count());

      roomId = this.roomService.HostRoom(user1);
      this.roomService.EnterUser(roomId, host, string.Empty);

      roundId = this.roomService.StartNewRound(roomId, user1.Id, new DefaultDeck());
      this.roomService.EndRound(roundId, user1.Id).Wait();

      Assert.AreEqual(3, this.results.GetList().Count());

    }
  }
}