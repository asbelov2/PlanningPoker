using Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoomApi.Controllers
{
  /// <summary>
  /// Round controller. Allows view round results and work with round logic.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class RoundController : ControllerBase
  {
    private RoundRepository rounds;
    private UserService userService;
    private RoomService roomService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoundController"/> class.
    /// </summary>
    /// <param name="roomService">Room service.</param>
    /// <param name="userService">User serivce.</param>
    /// <param name="roundRepository">Round repository.</param>
    public RoundController(RoomService roomService, UserService userService, RoundRepository roundRepository)
    {
      this.roomService = roomService;
      this.userService = userService;
      this.rounds = roundRepository;
    }

    /// <summary>
    /// Gets round results from one room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Round results from room.</returns>
    [HttpGet("RoundResult")]
    public ICollection<RoundResult> GetRoundResults(string roomId)
    {
      var roundResults = new RoundResultRepository();
      return roundResults.GetRoomRoundResults(Guid.Parse(roomId));
    }

    /// <summary>
    /// Gets round.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <returns>Round.</returns>
    [HttpGet("{id}")]
    public RoundDTO Get(string id)
    {
      if (this.rounds.GetItem(Guid.Parse(id)) != null)
      {
        return new RoundDTO(this.rounds.GetItem(Guid.Parse(id)));
      }

      return null;
    }

    /// <summary>
    /// Choose card.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="cardName">Card name.</param>
    [HttpPut("{id}/ChooseCard")]
    public void ChooseCard(string id, string userId, string cardName)
    {
      this.roomService.Choose(Guid.Parse(id), this.userService.GetUser(Guid.Parse(userId)), this.rounds.GetItem(Guid.Parse(id)).Deck.Cards.FirstOrDefault(x => x.Name == cardName));
    }

    /// <summary>
    /// Sets round title.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="title">New tite.</param>
    [HttpPut("{id}/SetTitle")]
    public void SetTitle(string id, string userId, string title)
    {
      this.roomService.SetRoundTitle(Guid.Parse(userId), Guid.Parse(id), title);
    }

    /// <summary>
    /// Sets round comment.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="comment">Comment.</param>
    [HttpPut("{id}/SetComment")]
    public void SetComment(string id, string userId, string comment)
    {
      this.roomService.SetRoundComment(Guid.Parse(userId), Guid.Parse(id), comment);
    }

    /// <summary>
    /// Ends round.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpPost("{id}/EndRound")]
    public void EndRound(string id, string userId)
    {
      this.roomService.EndRound(Guid.Parse(id), Guid.Parse(userId));
    }
  }
}