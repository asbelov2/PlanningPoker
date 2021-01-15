using System;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="RoomRepository"/> class. Stores rooms.
  /// </summary>
  public class RoomRepository : Repository<Room>
  {
    public RoomRepository(ApplicationContext context) : base(context)
    {

    }

    public Room GetByHostId(Guid id)
    {
      return this.GetItems(x => x.Host.Id == id).FirstOrDefault();
    }
  }
}