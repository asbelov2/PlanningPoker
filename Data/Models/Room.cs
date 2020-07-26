using System;
using System.Collections.Generic;

namespace Data
{
  /// <summary>
  /// <see cref="Room"/> class. Room must have host and can have name, password and card's interpretation.
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
    public Room(User host, string name = "Default name", string password = "", string cardInterpretation = "Hours")
    {
      this.Id = Guid.NewGuid();
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
    public ICollection<User> Users { get; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets room ID.
    /// </summary>
    public Guid Id { get; }

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