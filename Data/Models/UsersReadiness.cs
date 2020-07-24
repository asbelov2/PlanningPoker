using System;
using System.Collections.Generic;

namespace Data
{
  /// <summary>
  /// <see cref="UsersReadiness"/> class.
  /// </summary>
  /// <remarks>Contains readiness of all users of the room.</remarks>
  public class UsersReadiness : IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UsersReadiness"/> class.
    /// </summary>
    /// <param name="roomId">room ID</param>
    public UsersReadiness(string roomId)
    {
      this.IsUsersReady = new Dictionary<User, bool>();
      this.Id = Guid.NewGuid().ToString();
      this.RoomId = roomId;
    }

    /// <summary>
    /// Gets or sets user readiness in the room.
    /// </summary>
    public Dictionary<User, bool> IsUsersReady { get; }

    /// <summary>
    /// Gets ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets room ID.
    /// </summary>
    public string RoomId { get; }
  }
}