using System.Collections.Generic;
using Data;

namespace RoomApi
{
  /// <summary>
  /// Room data transfer object class. Used for transfering objects to client.
  /// </summary>
  public class RoomDTO
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RoomDTO"/> class.
    /// </summary>
    /// <param name="id">Room ID.</param>
    /// <param name="host">Host.</param>
    /// <param name="name">Room name.</param>
    /// <param name="cardInterpretation">Card's interpretation.</param>
    /// <param name="users">Collection of users.</param>
    public RoomDTO(string id, UserDTO host, string name, string cardInterpretation, List<UserDTO> users)
    {
      this.Id = id;
      this.CardInterpretation = cardInterpretation;
      this.Name = name;
      this.Host = host;
      this.Users = users;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomDTO"/> class.
    /// </summary>
    /// <param name="room">Room.</param>
    public RoomDTO(Room room)
    {
      this.Id = room.Id;
      this.CardInterpretation = room.CardInterpretation;
      this.Name = room.Name;
      this.Host = new UserDTO(room.Host);
      this.Users = new List<UserDTO>();
      foreach (var user in room.Users)
      {
        this.Users.Add(new UserDTO(user));
      }
    }

    /// <summary>
    /// Gets host.
    /// </summary>
    public UserDTO Host { get; set; }

    /// <summary>
    /// Gets collection of users.
    /// </summary>
    public ICollection<UserDTO> Users { get; }

    /// <summary>
    /// Gets room ID.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets room name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets card's interpretation.
    /// </summary>
    public string CardInterpretation { get; set; }
  }
}