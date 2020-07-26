using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="UserRepository"/> class. Stores all users.
  /// </summary>
  public class UserRepository : Repository<User>
  {
    /// <summary>
    /// Gets user by connection ID.
    /// </summary>
    /// <param name="connectionID">Connection ID.</param>
    /// <returns>User.</returns>
    public User GetByConnectionID(string connectionID)
    {
      return UserRepository.Data.FirstOrDefault(x => x.ConnectionId == connectionID);
    }
  }
}