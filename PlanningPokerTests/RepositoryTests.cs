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
    public void AddSameId()
    {
      Deck deck = new Deck("111");
      this.decks.Add(deck);
      deck = new Deck("111");
      this.decks.Add(deck);
      Assert.AreEqual(1, this.decks.GetList().Count());
    }

    [Test]
    public void WrongId()
    {
      Deck deck = new Deck("111");
      this.decks.Add(deck);
      Assert.IsNull(this.decks.GetItem("WrongID"));
    }

    [Test]
    public void GetRoomRoundResults()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      this.roomService.HostRoom("TestIDRoom", host);
      this.roomService.EnterUser("TestIDRoom", user1, string.Empty);

      this.roomService.StartNewRound("TestIDRoom", host.ConnectionId);
      this.roomService.EndRound(this.rounds.GetList().First().Id, host.ConnectionId);

      this.roomService.StartNewRound("TestIDRoom", host.ConnectionId);
      this.roomService.EndRound(this.rounds.GetList().ElementAt(1).Id, host.ConnectionId);

      this.roomService.HostRoom("TestID2Room", user1);
      this.roomService.EnterUser("TestID2Room", host, string.Empty);

      this.roomService.StartNewRound("TestID2Room", user1.ConnectionId);
      this.roomService.EndRound(this.rounds.GetList().FirstOrDefault(x => x.RoomId == "TestID2Room").Id, user1.ConnectionId);

      Assert.AreEqual(3, this.results.GetList().Count());
      Assert.AreEqual(2, this.results.GetRoomRoundResults("TestIDRoom").Count());
    }
  }
}