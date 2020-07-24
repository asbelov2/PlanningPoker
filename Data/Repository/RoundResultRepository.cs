﻿using System.Collections.Generic;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="RoundResultRepository"/> class.
  /// </summary>
  public class RoundResultRepository : Repository<RoundResult>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RoundResultRepository"/> class.
    /// </summary>
    public RoundResultRepository()
    {
    }

    /// <summary>
    /// Get round results in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Round results.</returns>
    public IEnumerable<RoundResult> GetRoomRoundResults(string roomId)
    {
      return data.Where(x => x.RoomId == roomId);
    }
  }
}
