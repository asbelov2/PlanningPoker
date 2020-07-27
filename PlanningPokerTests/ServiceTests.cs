using System;
using System.Linq;
using Data;
using Microsoft.AspNetCore.SignalR;
using NUnit.Framework;
using RoomApi;

namespace PlanningPokerTests
{
  internal class ServiceTests
  {
    private IHubContext<RoomHub> context;
    private DeckService deckService;
    private RoomService roomService;
    private RoundService roundService;
    private UserService userService;
    private DeckRepository decks;
    private RoomRepository rooms;
    private RoundRepository rounds;
    private UserRepository users;
    private RoundTimerRepository timers;
    private UsersReadinessRepository readiness;

    internal static string InvokedMethod { get; set; }

    internal static object[] Args { get; set; }

    [SetUp]
    public void Setup()
    {
      this.context = HubContextImplementation.GetContext;
      this.decks = new DeckRepository();
      this.rooms = new RoomRepository();
      this.rounds = new RoundRepository();
      this.users = new UserRepository();
      this.timers = new RoundTimerRepository();
      this.readiness = new UsersReadinessRepository();
      this.deckService = new DeckService(this.decks);
      this.userService = new UserService(this.users);
      var results = new RoundResultRepository();
      this.roundService = new RoundService(
        this.context,
        this.rooms,
        this.rounds,
        this.timers,
        results);
      this.roomService = new RoomService(
        this.context,
        this.roundService,
        this.userService,
        this.rooms,
        this.rounds,
        this.timers,
        this.readiness,
        results);
    }


    [TearDown]
    public void TearDown()
    {
      this.decks.ClearRepository();
      this.rooms.ClearRepository();
      this.rounds.ClearRepository();
      this.users.ClearRepository();
      this.timers.ClearRepository();
      this.readiness.ClearRepository();
    }

    [Test]
    public void NewDeckCreated()
    {
      var deckId = this.deckService.NewDeck("TestDeck");
      Assert.AreEqual("TestDeck", this.deckService.GetDeck(deckId).Name);
    }

    [Test]
    public void DeleteDeck()
    {
      var deckId = this.deckService.NewDeck("TestDeck");
      this.deckService.DeleteDeck(this.deckService.GetDeck(deckId));
      Assert.IsNull(this.deckService.GetDeck(deckId));
      deckId = this.deckService.NewDeck("TestDeck");
      this.deckService.DeleteDeck(deckId);
      Assert.IsNull(this.deckService.GetDeck(deckId));
    }

    [Test]
    public void AddCardAndGetDeck()
    {
      var deckId = this.deckService.NewDeck("TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Valuable, "321", 321));
      Assert.IsTrue(this.deckService.GetDeck(deckId).Cards.Count > 0);
    }

    [Test]
    public void RemoveCard()
    {
      var deckId = this.deckService.NewDeck("TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Valuable, "321", 321));
      this.deckService.RemoveCard(this.deckService.GetDeck(deckId), this.deckService.GetDeck(deckId).Cards.FirstOrDefault(x => x.Value == 321));
      Assert.IsFalse(this.deckService.GetDeck(deckId).Cards.Count > 0);
    }

    /// <summary>
    /// 3-rd deck - default.
    /// </summary>
    [Test]
    public void GetDecks()
    {
      this.deckService.NewDeck("123");
      this.deckService.NewDeck("4");
      Assert.AreEqual(2, this.deckService.GetDecks().Count());
    }

    [Test]
    public void NewUserAndGetUser()
    {
      var userId = this.userService.AddNewUser(new User("TestUser", "TestID"));
      Assert.AreEqual("TestUser", this.userService.GetUser(userId).Name);
    }

    [Test]
    public void DeleteUser()
    {
      var userId = this.userService.AddNewUser(new User("TestUser", "TestID"));
      this.userService.DeleteUser(this.userService.GetUser(userId));
      Assert.IsNull(this.userService.GetUser(userId));
      userId = this.userService.AddNewUser(new User("TestUser2", "TestID2"));
      this.userService.DeleteUser(userId);
      Assert.IsNull(this.userService.GetUser(userId));
    }

    public void GetUsers()
    {
      this.userService.AddNewUser(new User("TestUser", "TestID"));
      this.userService.AddNewUser(new User("TestUser2", "TestID2"));
      this.userService.AddNewUser(new User("TestUser3", "TestID3"));
      Assert.AreEqual(3, this.userService.GetUsers().Count());
    }

    [Test]
    public void CreateRoomAndConnectAndDisconnectUserAndPasswordCheck()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      User user2 = new User("User2", "TestIDUser2");
      User user3 = new User("User3", "TestIDUser3");
      var roomId = this.roomService.HostRoom(host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser(roomId , user1, "TestPassword");
      Assert.AreEqual("onConnected", InvokedMethod);
      Assert.AreEqual(typeof(Room), Args[0].GetType());
      this.roomService.EnterUser(roomId, user2, "TestPassword");
      this.roomService.EnterUser(roomId, user3, string.Empty);
      Assert.AreEqual("Host", this.rooms.GetItem(roomId).Host.Name);
      Assert.AreEqual(3, this.rooms.GetItem(roomId).Users.Count);
      Assert.AreEqual("TestRoomName", this.rooms.GetItem(roomId).Name);
      Assert.AreEqual("TestPassword", this.rooms.GetItem(roomId).Password);
      Assert.AreEqual("TestInterp", this.rooms.GetItem(roomId).CardInterpretation);
      this.roomService.LeaveUser(roomId, user2);
      Assert.AreEqual("onDisconnected", InvokedMethod);
      Assert.AreEqual(2, this.rooms.GetItem(roomId).Users.Count);
    }

    [Test]
    public void ReadyAndUnreadyAndAutostart()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      User user2 = new User("User2", "TestIDUser2");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      this.userService.AddNewUser(user2);
      var roomId = this.roomService.HostRoom(host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser(roomId, user1, "TestPassword");
      this.roomService.EnterUser(roomId, user2, "TestPassword");
      this.roomService.DeclareReady(roomId, host).Wait();
      Assert.AreEqual("onUserReady", InvokedMethod);
      this.roomService.DeclareReady(roomId, user1).Wait();
      this.roomService.DeclareNotReady(roomId, host).Wait();
      Assert.AreEqual("onUserNotReady", InvokedMethod);
      this.roomService.DeclareReady(roomId, user2).Wait();
      this.roomService.DeclareReady(roomId, host).Wait();
      Assert.AreEqual("onRoundStarted", InvokedMethod);
    }

    [Test]
    public void ChangeThingsInRoom()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      var roomId = this.roomService.HostRoom(host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser(roomId, user1, "TestPassword");
      this.roomService.ChangeCardInterpretation(roomId, host.Id, "NewCardInterp");
      Assert.AreEqual("NewCardInterp", this.rooms.GetItem(roomId).CardInterpretation);
      this.roomService.ChangePassword(roomId, host.Id, "NewPass");
      Assert.AreEqual("NewPass", this.rooms.GetItem(roomId).Password);
      this.roomService.ChangeRoomName(roomId, host.Id, "NewName");
      Assert.AreEqual("NewName", this.rooms.GetItem(roomId).Name);
      this.roomService.ChangeHost(roomId, host.Id, user1.Id);
      Assert.AreEqual("TestIDUser1", this.rooms.GetItem(roomId).Host.ConnectionId);
    }

    [Test]
    public void RoundStart()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      var roomId = this.roomService.HostRoom(host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser(roomId, user1, "TestPassword");
      var deckId = this.deckService.NewDeck("TestDeck");
      var roundId = this.roomService.StartNewRound(roomId, host.Id, this.deckService.GetDeck(deckId), "TestTitle", TimeSpan.FromMinutes(13));
      Assert.AreEqual("onRoundStarted", InvokedMethod);
      Round round = this.rounds.GetItem(roundId);
      Assert.NotNull(round.Id);
      Assert.NotNull(round.Duration);
      Assert.NotNull(round.StartDate);
      Assert.AreEqual(roomId, round.RoomId);
      Assert.IsNull(round.Result);
      Assert.AreEqual(new TimeSpan(0, 13, 0), round.RoundTime);
      Assert.AreEqual("TestTitle", round.Title);
      Assert.AreEqual("TestDeck", round.Deck.Name);
      Assert.AreEqual(2, round.Users.Count());
    }

    [Test]
    public void ChooseValuable()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      var roomId = this.roomService.HostRoom(host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser(roomId, user1, "TestPassword");
      var deckId = this.deckService.NewDeck("TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Valuable, "5", 5));
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Valuable, "15", 15));
      var roundId = this.roomService.StartNewRound(roomId, host.Id, this.deckService.GetDeck(deckId), "TestTitle", TimeSpan.FromMinutes(13));
      Round round = this.rounds.GetItem(roundId);
      this.roomService.Choose(roundId, host, new Card(CardType.Valuable, string.Empty, 123)).Wait();
      Assert.AreEqual("onWrongCard", InvokedMethod);
      this.roomService.Choose(roundId, host, this.deckService.GetDeck(deckId).Cards.ElementAt(0)).Wait();
      Assert.AreEqual("onUserChosed", InvokedMethod);
      this.roomService.Choose(roundId, user1, this.deckService.GetDeck(deckId).Cards.ElementAt(1)).Wait();
      Assert.AreEqual("onAllChosed", InvokedMethod);
      Assert.AreEqual(10, round.Result);
    }

    [Test]
    public void ChooseExceptional()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      var roomId = this.roomService.HostRoom(host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser(roomId, user1, "TestPassword");
      var deckId = this.deckService.NewDeck("TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Valuable, "5", 5));
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Exceptional, "ff", 0));
      var roundId = this.roomService.StartNewRound(roomId, host.Id, this.deckService.GetDeck(deckId), "TestTitle", TimeSpan.FromMinutes(13));
      Round round = this.rounds.GetItem(roundId);
      this.roomService.Choose(round.Id, host, this.deckService.GetDeck(deckId).Cards.ElementAt(0)).Wait();
      this.roomService.Choose(round.Id, user1, this.deckService.GetDeck(deckId).Cards.ElementAt(1)).Wait();
      Assert.IsNull(round.Result);
    }

    [Test]
    public void EndRound()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      var roomId = this.roomService.HostRoom(host);
      this.roomService.EnterUser(roomId, user1, string.Empty);
      var roundId = this.roomService.StartNewRound(roomId, host.Id, new DefaultDeck());
      this.roomService.EndRound(roundId, host.Id).Wait();
      Assert.AreEqual("onEnd", InvokedMethod);
      Assert.AreEqual(typeof(RoundDTO), Args[0].GetType());
    }

    [Test]
    public void ChangeThingsInRound()
    {
      this.rounds.ClearRepository();
      this.rounds = new RoundRepository();
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      var roomId = this.roomService.HostRoom(host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser(roomId, user1, "TestPassword");
      var deckId = this.deckService.NewDeck("TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Valuable, "5", 5));
      this.deckService.AddCard(this.deckService.GetDeck(deckId), new Card(CardType.Valuable, "15", 15));
      var roundId = this.roomService.StartNewRound(roomId, host.Id, this.deckService.GetDeck(deckId), "TestTitle", TimeSpan.FromMinutes(13));
      Round round = this.rounds.GetItem(roundId);
      this.roomService.SetRoundTitle(host.Id, roundId, "TestTitle");
      this.roomService.SetRoundComment(host.Id, roundId, "TestComment");
      Assert.AreEqual("TestTitle", round.Title);
      Assert.AreEqual("TestComment", round.Comment);
    }
  }
}