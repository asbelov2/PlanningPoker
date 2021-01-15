using System;

namespace Data
{
  /// <summary>
  /// <see cref="User"/> class.
  /// </summary>
  public class User : IEntity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="name">Username.</param>
    /// <param name="connectionId">user ID.</param>
    public User(string name, string connectionId)
    {
      this.Name = name;
      this.ConnectionId = connectionId;
      this.Id = Guid.NewGuid();
    }

    /// <summary>
    /// User ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User connection ID.
    /// </summary>
    public string ConnectionId { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    public string Name { get; set; }
  }
}