using System;
using System.Collections.Generic;

namespace Data
{
  /// <summary>
  /// <see cref="Room"/> class.
  /// </summary>
  public class Room : IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Room"/> class.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="host">Host of the room.</param>
    /// <param name="name">Name of the room.</param>
    /// <param name="password">Password of the room.</param>
    /// <param name="cardInterpretation">Card's interpretation.</param>
    public Room(string id, User host, string name = "Default name", string password = "", string cardInterpretation = "Hours")
    {
      if (id.Length <= 0)
      {
        throw new Exception("Id can't be empty");
      }

      this.Id = id;
      this.Password = password;
      this.CardInterpretation = cardInterpretation;
      this.Name = name;
      this.Host = host;
      this.Users = new List<User>();
      this.Users.Add(this.Host);
    }

    /// <summary>
    /// Gets or sets host.
    /// </summary>
    public User Host { get; set; }

    /// <summary>
    /// Gets collection of users in room.
    /// </summary>
    public List<User> Users { get; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets room ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets or sets room name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets card's interpretation
    /// </summary>
    public string CardInterpretation { get; set; }
  }
}