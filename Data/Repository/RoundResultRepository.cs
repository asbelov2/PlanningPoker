using System.Collections.Generic;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="RoundResultRepository"/> class.
  /// </summary>
  public class RoundResultRepository : Repository<RoundResult>
  {
    /// <summary>
    /// Get round results in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Round results.</returns>
    public ICollection<RoundResult> GetRoomRoundResults(string roomId)
    {
      return Data.Where(x => x.RoomId == roomId).ToList();
    }
  }
}
