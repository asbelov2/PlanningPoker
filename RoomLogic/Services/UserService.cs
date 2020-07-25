using System.Collections.Generic;
using Data;

namespace RoomApi
{
  /// <summary>
  /// UserService class.
  /// </summary>
  public class UserService
  {
    private static UserRepository users;

    /// <summary>
    /// Initializes static members of the <see cref="UserService"/> class.
    /// </summary>
    static UserService()
    {
      users = new UserRepository();
    }

    /// <summary>
    /// Add new user in repository.
    /// </summary>
    /// <param name="user">User.</param>
    public void AddNewUser(User user)
    {
      users.Add(user);
    }

    /// <summary>
    /// Get all users from repository.
    /// </summary>
    /// <returns>Collection of users.</returns>
    public ICollection<User> GetUsers()
    {
      return users.GetList();
    }

    /// <summary>
    /// Get user from repository.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>User.</returns>
    public User GetUser(string id)
    {
      return users.GetItem(id);
    }

    /// <summary>
    /// Delete user from repository.
    /// </summary>
    /// <param name="user">User.</param>
    public void DeleteUser(User user)
    {
      users.Delete(user);
    }

    /// <summary>
    /// Delete user from repository by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    public void DeleteUser(string id)
    {
      users.Delete(users.GetItem(id));
    }
  }
}