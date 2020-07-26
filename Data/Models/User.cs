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
    /// Gets user ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets or sets user connection ID.
    /// </summary>
    public string ConnectionId { get; set; }

    /// <summary>
    /// Gets or sets username.
    /// </summary>
    public string Name { get; set; }
  }
}