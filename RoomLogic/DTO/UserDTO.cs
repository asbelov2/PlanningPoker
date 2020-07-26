using Data;
using System;

namespace RoomApi
{
  /// <summary>
  /// User data transfer object class. Used for transfering objects to client.
  /// </summary>
  public class UserDTO
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDTO"/> class.
    /// </summary>
    /// <param name="name">User name.</param>
    /// <param name="connectionId">User connection ID.</param>
    /// <param name="id">User ID.</param>
    public UserDTO(string name, string connectionId, Guid id)
    {
      this.ConntectionId = connectionId;
      this.Id = id;
      this.Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDTO"/> class.
    /// </summary>
    /// <param name="user">User.</param>
    public UserDTO(User user)
    {
      this.ConntectionId = user.ConnectionId;
      this.Id = user.Id;
      this.Name = user.Name;
    }

    /// <summary>
    /// Gets user connection ID.
    /// </summary>
    public string ConntectionId { get; }

    /// <summary>
    /// Gets user ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets user name.
    /// </summary>
    public string Name { get; }
  }
}