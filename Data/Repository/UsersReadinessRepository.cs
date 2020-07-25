using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="UsersReadinessRepository"/> class.
  /// </summary>
  public class UsersReadinessRepository : Repository<UsersReadiness>
  {
    /// <summary>
    /// Gets item by room ID.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    /// <returns>Users readiness in that room.</returns>
    public UsersReadiness GetItemByRoomId(string roomId)
    {
      return Data.FirstOrDefault(x => x.RoomId == roomId);
    }

    /// <summary>
    /// Turn to false all readiness in room.
    /// </summary>
    /// <param name="roomId">Room ID.</param>
    public void TurnToFalse(string roomId)
    {
      var dict = Data.FirstOrDefault(x => x.RoomId == roomId).IsUsersReady;
      foreach (var key in dict.Keys.Cast<User>().ToArray())
      {
        dict[key] = false;
      }
    }
  }
}