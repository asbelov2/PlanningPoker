using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="UserRepository"/> class. Stores all users.
  /// </summary>
  public class UserRepository : Repository<User>
  {
    public UserRepository(ApplicationContext context) : base(context)
    {

    }

    /// <summary>
    /// Gets user by connection ID.
    /// </summary>
    /// <param name="connectionID">Connection ID.</param>
    /// <returns>User.</returns>
    public User GetByConnectionID(string connectionID)
    {
      return this.GetItems(x => x.ConnectionId == connectionID).FirstOrDefault();
    }
  }
}