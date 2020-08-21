﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Microsoft.AspNetCore.Mvc;

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
    private RoundService roundService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoundController"/> class.
    /// </summary>
    /// <param name="roomService">Room service.</param>
    /// <param name="userService">User serivce.</param>
    /// <param name="roundRepository">Round repository.</param>
    public RoundController(RoomService roomService, RoundService roundService, UserService userService, RoundRepository roundRepository)
    {
      this.roomService = roomService;
      this.roundService = roundService;
      this.userService = userService;
      this.rounds = roundRepository;
    }

    /// <summary>
    /// Gets round results from one room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Round results from room.</returns>
    [HttpGet("RoundResult")]
    public IEnumerable<RoundDTO> GetRoundResults(Guid roomId)
    {
      return roundService.GetRoundResultsByRoomId(roomId);
    }

    /// <summary>
    /// Gets round.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <returns>Round.</returns>
    [HttpGet("{id}")]
    public RoundDTO Get(Guid id)
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
    [HttpPut("{id}/ChooseCard")]
    public void ChooseCard(Guid id, Guid userId, string cardName)
    {
      this.roomService.Choose(id, this.userService.GetUser(userId), this.rounds.GetItem(id).Deck.Cards.FirstOrDefault(x => x.Name == cardName));
    }

    /// <summary>
    /// Sets round title.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="title">New tite.</param>
    [HttpPut("{id}/SetTitle")]
    public void SetTitle(Guid id, Guid userId, string title)
    {
      this.roundService.SetRoundTitle(userId, id, title);
    }

    /// <summary>
    /// Sets round comment.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="comment">Comment.</param>
    [HttpPut("{id}/SetComment")]
    public void SetComment(Guid id, Guid userId, string comment)
    {
      this.roundService.SetRoundComment(userId, id, comment);
    }

    /// <summary>
    /// Ends round.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpPost("{id}/EndRound")]
    public void EndRound(Guid id, Guid userId)
    {
      this.roomService.EndRound(id, userId);
    }

    /// <summary>
    /// Resets timer.
    /// </summary>
    /// <param name="id">Round ID.</param>
    /// <param name="userId">User ID.</param>
    [HttpPost("{id}/ResetTimer")]
    public void ResetTimer(Guid id, Guid userId)
    {
      this.roundService.ResetTimer(id, userId);
    }
  }
}