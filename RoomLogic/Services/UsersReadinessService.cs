using System;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace RoomApi
{
  public class UsersReadinessService
  {
    private static ICollection<UsersReadiness> data = new List<UsersReadiness>();

    /// <summary>
    /// Add readiness.
    /// </summary>
    /// <param name="readiness">Readiness.</param>
    /// <returns>Room ID.</returns>
    public Guid Add(UsersReadiness readiness)
    {
      if (!data.Any(x => x.RoomId == readiness.RoomId))
      { 
        data.Add(readiness);
        return readiness.RoomId;
      }

      return default;
    }

    /// <summary>
    /// Gets item by room ID.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Users readiness in that room.</returns>
    public UsersReadiness GetItemByRoomId(Guid roomId)
    {
      return data.FirstOrDefault(x => x.RoomId == roomId);
    }

    /// <summary>
    /// Delete readiness.
    /// </summary>
    /// <param name="readiness">Readiness.</param>
    public void Delete(UsersReadiness readiness)
    {
      if (readiness != null)
      {
        data.Remove(readiness);
      }
    }

    /// <summary>
    /// Return collection of readinesss.
    /// </summary>
    /// <returns>collection of readinesss.</returns>
    public IEnumerable<UsersReadiness> GetList()
    {
      return data;
    }
  }
}
