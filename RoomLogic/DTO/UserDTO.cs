﻿using Data;

namespace RoomApi
{
  /// <summary>
  /// UserDTO class.
  /// </summary>
  public class UserDTO
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDTO"/> class.
    /// </summary>
    /// <param name="name">User name.</param>
    /// <param name="id">User ID.</param>
    public UserDTO(string name, string id)
    {
      this.Id = id;
      this.Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDTO"/> class.
    /// </summary>
    /// <param name="user">User.</param>
    public UserDTO(User user)
    {
      this.Id = user.Id;
      this.Name = user.Name;
    }

    /// <summary>
    /// Gets user ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets user name.
    /// </summary>
    public string Name { get; }
  }
}