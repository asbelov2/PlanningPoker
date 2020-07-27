using System;
using System.Collections.Generic;

namespace Data
{
  /// <summary>
  /// <see cref="UsersReadiness"/> class. Presents users readiness in room. Used for automatic round start in rooms.
  /// </summary>
  /// <remarks>Contains readiness of all users of the room.</remarks>
  public class UsersReadiness
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UsersReadiness"/> class.
    /// </summary>
    /// <param name="roomId">room ID</param>
    public UsersReadiness(Guid roomId)
    {
      this.IsUsersReady = new Dictionary<User, bool>();
      this.Id = Guid.NewGuid();
      this.RoomId = roomId;
    }

    /// <summary>
    /// User readiness in the room.
    /// </summary>
    public Dictionary<User, bool> IsUsersReady { get; }

    /// <summary>
    /// ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Room ID.
    /// </summary>
    public Guid RoomId { get; }
  }
}