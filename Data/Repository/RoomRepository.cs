using System;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="RoomRepository"/> class. Stores rooms.
  /// </summary>
  public class RoomRepository : Repository<Room>
  {
    public Room GetByHostId(Guid id)
    {
      return Data.FirstOrDefault(x => x.Host.Id == id);
    }
  }
}