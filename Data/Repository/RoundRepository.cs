using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="RoundRepository"/> class. Stores rounds (even if they ended).
  /// </summary>
  public class RoundRepository : Repository<Round>
  {
    public RoundRepository(ApplicationContext context) : base(context)
    {

    }

    /// <summary>
    /// Get round results in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Round results.</returns>
    public ICollection<Round> GetRoomRoundResults(Guid roomId)
    {
      return this.GetItems(x => x.RoomId == roomId).ToList();
    }
  }
}