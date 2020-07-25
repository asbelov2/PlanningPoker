using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace RoomApi.Controllers
{
  /// <summary>
  /// Room controller.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class RoomController : ControllerBase
  {
    private RoomRepository rooms = new RoomRepository();
    private DeckRepository decks = new DeckRepository();
    private RoomService roomService;
    private UserService userService = new UserService();
    private IHubContext<RoomHub> hubContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomController"/> class.
    /// </summary>
    /// <param name="hubContext">Hub context</param>
    public RoomController(IHubContext<RoomHub> hubContext)
    {
      this.hubContext = hubContext;
      this.roomService = new RoomService(hubContext);
    }

    /// <summary>
    /// Get all rooms.
    /// </summary>
    /// <returns>All rooms.</returns>
    [HttpGet]
    public ICollection<RoomDTO> Get()
    {
      var result = new List<RoomDTO>();
      foreach (var room in this.rooms.GetList())
      {
        if (room != null)
        {
          result.Add(new RoomDTO(room));
        }
      }
      return result;
    }

    /// <summary>
    /// Get one room by id.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <returns>Room.</returns>
    [HttpGet("{id}")]
    public RoomDTO Get(string id)
    {
      if (this.rooms.GetItem(id) != null)
      {
        return new RoomDTO(this.rooms.GetItem(id));
      }
      return null;
    }

    /// <summary>
    /// Connecting user to room.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="password">Room password.</param>
    [HttpPost("{id}/Connect")]
    public void Connect(string id, string userId, string password)
    {
      this.roomService.EnterUser(id, this.userService.GetUser(userId), password);
    }

    /// <summary>
    /// Disconnecting user from the room.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpPost("{id}/Disconnect")]
    public void Disconnect(string id, string userId)
    {
      this.roomService.LeaveUser(id, this.userService.GetUser(userId));
    }

    /// <summary>
    /// Creating room.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="hostId">Host ID.</param>
    /// <param name="name">Room name.</param>
    /// <param name="password">Room password.</param>
    /// <param name="cardInterpretation">Room card's interpretation.</param>
    [HttpPost]
    public void Create(string id, string hostId, string name = "Default name", string password = "", string cardInterpretation = "Hours")
    {
      this.roomService.HostRoom(id, this.userService.GetUser(hostId), name, password, cardInterpretation);
    }

    /// <summary>
    /// Sets card's interpreatation.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="cardInterpretation">Room card's interpretation</param>
    [HttpPut("{id}/SetCardInterpretation")]
    public void SetCardInterpretation(string id, string userId, string cardInterpretation)
    {
      this.roomService.ChangeCardInterpretation(id, userId, cardInterpretation);
    }

    /// <summary>
    /// Starts new round
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="title">Round title.</param>
    /// <param name="deckId">Deck ID.</param>
    /// <param name="roundTimeInMinutes">Round time in minutes.</param>
    [HttpPost("{id}/StartRound")]
    public void StartRound(string id, string userId, string title, string deckId, double roundTimeInMinutes)
    {
      this.roomService.StartNewRound(id, userId, title, this.decks.GetItem(deckId), TimeSpan.FromMinutes(roundTimeInMinutes));
    }

    /// <summary>
    /// Sets room name.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="name">Room new name.</param>
    [HttpPut("{id}/SetName")]
    public void SetName(string id, string userId, string name)
    {
      this.roomService.ChangeRoomName(id, userId, name);
    }

    /// <summary>
    /// Sets new host
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="hostId">New host ID.</param>
    [HttpPut("{id}/SetHost")]
    public void SetHost(string id, string userId, string hostId)
    {
      this.roomService.ChangeHost(id, userId, hostId);
    }

    /// <summary>
    /// Sets new password
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="password">Room password.</param>
    [HttpPut("{id}/SetPassword")]
    public void SetPassword(string id, string userId, string password)
    {
      this.roomService.ChangePassword(id, userId, password);
    }

    /// <summary>
    /// Declare user's readiness.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <returns>Async task.</returns>
    [HttpPut("{id}/DeclareReady")]
    public async Task DeclareReady(string id, string userId)
    {
      await this.roomService.DeclareReady(id, this.userService.GetUser(userId));
    }

    /// <summary>
    /// Declare user's not readiness.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <returns>Async task.</returns>
    [HttpPut("{id}/DeclareNotReady")]
    public async Task DeclareNotReady(string id, string userId)
    {
      await this.roomService.DeclareNotReady(id, this.userService.GetUser(userId));
    }

    /// <summary>
    /// Delete room
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpDelete("{id}")]
    public void Delete(string id, string userId)
    {
      if (userId == this.rooms.GetItem(id).Host.Id)
      {
        this.rooms.Delete(this.rooms.GetItem(id));
      }
    }
  }
}