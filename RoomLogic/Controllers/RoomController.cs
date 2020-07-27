using Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomApi.Controllers
{
  /// <summary>
  /// Room controller. Main controller for all room logic.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class RoomController : ControllerBase
  {
    private RoomRepository rooms;
    private DeckRepository decks;
    private RoomService roomService;
    private UserService userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomController"/> class.
    /// </summary>
    /// <param name="userService">User service.</param>
    /// <param name="roomService">Room service.</param>
    /// <param name="roomRepository">Room repository.</param>
    /// <param name="deckRepository">Deck repository.</param>
    public RoomController(
      UserService userService,
      RoomService roomService,
      RoomRepository roomRepository,
      DeckRepository deckRepository)
    {
      this.roomService = roomService;
      this.userService = userService;
      this.rooms = roomRepository;
      this.decks = deckRepository;
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
    public RoomDTO Get(Guid id)
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
    public void Connect(Guid id, Guid userId, string password)
    {
      this.roomService.EnterUser(id, this.userService.GetUser(userId), password);
    }

    /// <summary>
    /// Disconnecting user from the room.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpPost("{id}/Disconnect")]
    public void Disconnect(Guid id, Guid userId)
    {
      this.roomService.LeaveUser(id, this.userService.GetUser(userId));
    }

    /// <summary>
    /// Creating room.
    /// </summary>
    /// <param name="hostId">Host ID.</param>
    /// <param name="name">Room name.</param>
    /// <param name="password">Room password.</param>
    /// <param name="cardInterpretation">Room card's interpretation.</param>
    [HttpPost]
    public void Create(Guid hostId, string name = "Default name", string password = "", string cardInterpretation = "Hours")
    {
      this.roomService.HostRoom(this.userService.GetUser(hostId), name, password, cardInterpretation);
    }

    /// <summary>
    /// Sets card's interpreatation.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="cardInterpretation">Room card's interpretation</param>
    [HttpPut("{id}/SetCardInterpretation")]
    public void SetCardInterpretation(Guid id, Guid userId, string cardInterpretation)
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
    public void StartRound(Guid id, Guid userId, string title, Guid deckId, double roundTimeInMinutes)
    {
        this.roomService.StartNewRound(id, userId, this.decks.GetItem(deckId), title, TimeSpan.FromMinutes(roundTimeInMinutes));
    }

    /// <summary>
    /// Sets room name.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="name">Room new name.</param>
    [HttpPut("{id}/SetName")]
    public void SetName(Guid id, Guid userId, string name)
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
    public void SetHost(Guid id, Guid userId, string hostId)
    {
      this.roomService.ChangeHost(id, userId, Guid.Parse(hostId));
    }

    /// <summary>
    /// Sets new password
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="password">Room password.</param>
    [HttpPut("{id}/SetPassword")]
    public void SetPassword(Guid id, Guid userId, string password)
    {
      this.roomService.ChangePassword(id, userId, password);
    }

    /// <summary>
    /// Declare user's readiness.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpPut("{id}/DeclareReady")]
    public void DeclareReady(string id, string userId)
    {
      this.roomService.DeclareReady(Guid.Parse(id), this.userService.GetUser(Guid.Parse(userId)));
    }

    /// <summary>
    /// Declare user's not readiness.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpPut("{id}/DeclareNotReady")]
    public void DeclareNotReady(Guid id, Guid userId)
    {
      this.roomService.DeclareNotReady(id, this.userService.GetUser(userId));
    }

    /// <summary>
    /// Delete room
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpDelete("{id}")]
    public void Delete(Guid id, Guid userId)
    {
      if (userId == this.rooms.GetItem(id).Host.Id)
      {
        this.rooms.Delete(this.rooms.GetItem(id));
      }
    }
  }
}