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
    public static string InvokedMethod = string.Empty;
    public static object[] Args;
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



    [SetUp]
    public void Setup()
    {
      this.context = HubContextImplementation.GetContext;
      this.deckService = new DeckService();
      this.roomService = new RoomService(this.context);
      this.roundService = new RoundService(this.context);
      this.userService = new UserService();
      this.decks = new DeckRepository();
      this.rooms = new RoomRepository();
      this.rounds = new RoundRepository();
      this.users = new UserRepository();
      this.timers = new RoundTimerRepository();
      this.readiness = new UsersReadinessRepository();
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
      this.deckService.NewDeck("123", "TestDeck");
      Assert.AreEqual("TestDeck", this.deckService.GetDeck("123").Name);
    }

    [Test]
    public void DeleteDeck()
    {
      this.deckService.NewDeck("123", "TestDeck");
      this.deckService.DeleteDeck(this.deckService.GetDeck("123"));
      Assert.IsNull(this.deckService.GetDeck("123"));
      this.deckService.NewDeck("456", "TestDeck");
      this.deckService.DeleteDeck("456");
      Assert.IsNull(this.deckService.GetDeck("456"));
    }

    [Test]
    public void AddCardAndGetDeck()
    {
      this.deckService.NewDeck("123", "TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck("123"), new Card(CardType.Valuable, "321", 321));
      Assert.IsTrue(this.deckService.GetDeck("123").Cards.Count > 0);
    }

    [Test]
    public void RemoveCard()
    {
      this.deckService.NewDeck("123", "TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck("123"), new Card(CardType.Valuable, "321", 321));
      this.deckService.RemoveCard(this.deckService.GetDeck("123"), this.deckService.GetDeck("123").Cards.FirstOrDefault(x => x.Value == 321));
      Assert.IsFalse(this.deckService.GetDeck("123").Cards.Count > 0);
    }

    /// <summary>
    /// 3-rd deck - default.
    /// </summary>
    [Test]
    public void GetDecks()
    {
      this.deckService.NewDeck("123", "123");
      this.deckService.NewDeck("4", "4");
      Assert.AreEqual(3, this.deckService.GetDecks().Count());
    }

    [Test]
    public void NewUserAndGetUser()
    {
      this.userService.AddNewUser(new User("TestUser", "TestID"));
      Assert.AreEqual("TestUser", this.userService.GetUser("TestID").Name);
    }

    [Test]
    public void DeleteUser()
    {
      this.userService.AddNewUser(new User("TestUser", "TestID"));
      this.userService.DeleteUser(this.userService.GetUser("TestID"));
      Assert.IsNull(this.userService.GetUser("TestID"));
      this.userService.AddNewUser(new User("TestUser2", "TestID2"));
      this.userService.DeleteUser("TestID2");
      Assert.IsNull(this.userService.GetUser("TestID2"));
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
      this.roomService.HostRoom("TestIDRoom", host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser("TestIDRoom", user1, "TestPassword");
      Assert.AreEqual("onConnected", InvokedMethod);
      Assert.AreEqual(typeof(Room), Args[0].GetType());
      this.roomService.EnterUser("TestIDRoom", user2, "TestPassword");
      this.roomService.EnterUser("TestIDRoom", user3, string.Empty);
      Assert.AreEqual("Host", this.rooms.GetItem("TestIDRoom").Host.Name);
      Assert.AreEqual(3, this.rooms.GetItem("TestIDRoom").Users.Count);
      Assert.AreEqual("TestRoomName", this.rooms.GetItem("TestIDRoom").Name);
      Assert.AreEqual("TestPassword", this.rooms.GetItem("TestIDRoom").Password);
      Assert.AreEqual("TestInterp", this.rooms.GetItem("TestIDRoom").CardInterpretation);
      this.roomService.LeaveUser("TestIDRoom", user2);
      Assert.AreEqual("onDisconnected", InvokedMethod);
      Assert.AreEqual(2, this.rooms.GetItem("TestIDRoom").Users.Count);
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
      this.roomService.HostRoom("TestIDRoom", host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser("TestIDRoom", user1, "TestPassword");
      this.roomService.EnterUser("TestIDRoom", user2, "TestPassword");
      this.roomService.DeclareReady("TestIDRoom", host).Wait();
      Assert.AreEqual("onUserReady", InvokedMethod);
      this.roomService.DeclareReady("TestIDRoom", user1).Wait();
      this.roomService.DeclareNotReady("TestIDRoom", host).Wait();
      Assert.AreEqual("onUserNotReady", InvokedMethod);
      this.roomService.DeclareReady("TestIDRoom", user2).Wait();
      this.roomService.DeclareReady("TestIDRoom", host).Wait();
      Assert.AreEqual("onRoundStarted", InvokedMethod);
    }

    [Test]
    public void ChangeThingsInRoom()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      this.roomService.HostRoom("TestIDRoom", host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser("TestIDRoom", user1, "TestPassword");
      this.roomService.ChangeCardInterpretation("TestIDRoom", host.Id, "NewCardInterp");
      Assert.AreEqual("NewCardInterp", this.rooms.GetItem("TestIDRoom").CardInterpretation);
      this.roomService.ChangePassword("TestIDRoom", host.Id, "NewPass");
      Assert.AreEqual("NewPass", this.rooms.GetItem("TestIDRoom").Password);
      this.roomService.ChangeRoomName("TestIDRoom", host.Id, "NewName");
      Assert.AreEqual("NewName", this.rooms.GetItem("TestIDRoom").Name);
      this.roomService.ChangeHost("TestIDRoom", host.Id, user1.Id);
      Assert.AreEqual("TestIDUser1", this.rooms.GetItem("TestIDRoom").Host.Id);
    }

    [Test]
    public void RoundStart()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      this.roomService.HostRoom("TestIDRoom", host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser("TestIDRoom", user1, "TestPassword");
      this.deckService.NewDeck("1", "TestDeck");
      this.roomService.StartNewRound("TestIDRoom", host.Id, "TestTitle", this.deckService.GetDeck("1"), TimeSpan.FromMinutes(13));
      Assert.AreEqual("onRoundStarted", InvokedMethod);
      Round round = this.rounds.GetList().First();
      Assert.NotNull(round.Id);
      Assert.NotNull(round.Duration);
      Assert.NotNull(round.StartDate);
      Assert.AreEqual("TestIDRoom", round.RoomId);
      Assert.AreEqual(0, round.Result);
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
      this.roomService.HostRoom("TestIDRoom", host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser("TestIDRoom", user1, "TestPassword");
      this.deckService.NewDeck("1", "TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck("1"), new Card(CardType.Valuable, "5", 5));
      this.deckService.AddCard(this.deckService.GetDeck("1"), new Card(CardType.Valuable, "15", 15));
      this.roomService.StartNewRound("TestIDRoom", host.Id, "TestTitle", this.deckService.GetDeck("1"), TimeSpan.FromMinutes(13));
      Round round = this.rounds.GetList().First();
      this.roomService.Choose(round.Id, host, new Card(CardType.Valuable, string.Empty, 123)).Wait();
      Assert.AreEqual("onWrongCard", InvokedMethod);
      this.roomService.Choose(round.Id, host, this.deckService.GetDeck("1").Cards.ElementAt(0)).Wait();
      Assert.AreEqual("onUserChosed", InvokedMethod);
      this.roomService.Choose(round.Id, user1, this.deckService.GetDeck("1").Cards.ElementAt(1)).Wait();
      Assert.AreEqual("onAllChosed", InvokedMethod);
      Assert.AreEqual(10.0, round.Result);
    }

    [Test]
    public void ChooseExceptional()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      this.roomService.HostRoom("TestIDRoom", host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser("TestIDRoom", user1, "TestPassword");
      this.deckService.NewDeck("1", "TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck("1"), new Card(CardType.Valuable, "5", 5));
      this.deckService.AddCard(this.deckService.GetDeck("1"), new Card(CardType.Exceptional, "ff", 0));
      this.roomService.StartNewRound("TestIDRoom", host.Id, "TestTitle", this.deckService.GetDeck("1"), TimeSpan.FromMinutes(13));
      Round round = this.rounds.GetList().First();
      this.roomService.Choose(round.Id, host, this.deckService.GetDeck("1").Cards.ElementAt(0)).Wait();
      this.roomService.Choose(round.Id, user1, this.deckService.GetDeck("1").Cards.ElementAt(1)).Wait();
      Assert.AreEqual(-1.0, round.Result);
    }

    [Test]
    public void EndRound()
    {
      User host = new User("Host", "TestIDHost");
      User user1 = new User("User1", "TestIDUser1");
      this.userService.AddNewUser(host);
      this.userService.AddNewUser(user1);
      this.roomService.HostRoom("TestIDRoom", host);
      this.roomService.EnterUser("TestIDRoom", user1, string.Empty);
      var roundId = this.roomService.StartNewRound("TestIDRoom", host.Id);
      Round round = this.rounds.GetItem(roundId);
      this.roomService.EndRound(round.Id, host.Id).Wait();
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
      this.roomService.HostRoom("TestIDRoom", host, "TestRoomName", "TestPassword", "TestInterp");
      this.roomService.EnterUser("TestIDRoom", user1, "TestPassword");
      this.deckService.NewDeck("1", "TestDeck");
      this.deckService.AddCard(this.deckService.GetDeck("1"), new Card(CardType.Valuable, "5", 5));
      this.deckService.AddCard(this.deckService.GetDeck("1"), new Card(CardType.Valuable, "15", 15));
      this.roomService.StartNewRound("TestIDRoom", host.Id, "TestTitle", this.deckService.GetDeck("1"), TimeSpan.FromMinutes(13));
      Round round = this.rounds.GetList().First();
      this.roomService.SetRoundTitle(host.Id, round.Id, "TestTitle");
      this.roomService.SetRoundComment(host.Id, round.Id, "TestComment");
      Assert.AreEqual("TestTitle", round.Title);
      Assert.AreEqual("TestComment", round.Comment);
    }
  }
}