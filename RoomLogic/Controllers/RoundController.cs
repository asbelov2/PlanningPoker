﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace RoomApi.Controllers
{
  /// <summary>
  /// Round controller.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class RoundController : ControllerBase
  {
    private RoundRepository rounds = new RoundRepository();
    private UserService userService = new UserService();
    private RoomService roomService;
    private IHubContext<RoomHub> hubContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomController"/> class.
    /// </summary>
    /// <param name="hubContext">Hub context</param>
    public RoundController(IHubContext<RoomHub> hubContext)
    {
      this.hubContext = hubContext;
      this.roomService = new RoomService(hubContext);
    }

    /// <summary>
    /// Gets round results from one room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Round results from room.</returns>
    [HttpGet("RoundResult")]
    public IEnumerable<RoundResult> GetRoundResults(string roomId)
    {
      var roundResults = new RoundResultRepository();
      return roundResults.GetRoomRoundResults(roomId);
    }

    /// <summary>
    /// Gets round.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <returns>Round.</returns>
    [HttpGet("{id}")]
    public RoundDTO Get(string id)
    {
      if (this.rounds.GetItem(id) != null)
      {
        return new RoundDTO(this.rounds.GetItem(id));
      }

      return null;
    }

    /// <summary>
    /// Choose card.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="cardName">Card name.</param>
    /// <returns>Async task.</returns>
    [HttpPut("{id}/ChooseCard")]
    public async Task ChooseCard(string id, string userId, string cardName)
    {
      await this.roomService.Choose(id, this.userService.GetUser(userId), this.rounds.GetItem(id).Deck.Cards.FirstOrDefault(x => x.Name == cardName));
    }

    /// <summary>
    /// Sets round title.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="title">New tite.</param>
    /// <returns>Async task.</returns>
    [HttpPut("{id}/SetTitle")]
    public async Task SetTitle(string id, string userId, string title)
    {
      await this.roomService.SetRoundTitle(userId, id, title);
    }

    /// <summary>
    /// Sets round comment.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="comment">Comment.</param>
    /// <returns>Async task.</returns>
    [HttpPut("{id}/SetComment")]
    public async Task SetComment(string id, string userId, string comment)
    {
      await this.roomService.SetRoundComment(userId, id, comment);
    }

    /// <summary>
    /// Ends round.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <returns>Async task.</returns>
    [HttpPost("{id}/EndRound")]
    public async Task EndRound(string id, string userId)
    {
      await this.roomService.EndRound(id, userId);
    }
  }
}