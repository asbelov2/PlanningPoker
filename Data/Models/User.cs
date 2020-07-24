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
    }

    /// <summary>
    /// Gets or sets connection ID of user.
    /// </summary>
    public string ConnectionId { get; set; }

    /// <summary>
    /// Gets or sets user ID.
    /// </summary>
    /// <remarks>Equal to connectionId.</remarks>
    public string Id
    {
      get
      {
        return this.ConnectionId;
      }
    }

    /// <summary>
    /// Gets or sets username.
    /// </summary>
    public string Name { get; set; }
  }
}