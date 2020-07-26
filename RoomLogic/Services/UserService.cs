using System;
using System.Collections.Generic;
using Data;

namespace RoomApi
{
  /// <summary>
  /// UserService class. Allows work with users.
  /// </summary>
  public class UserService
  {
    private UserRepository users;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">Users repository.</param>
    public UserService(UserRepository userRepository)
    {
      this.users = userRepository;
    }

    /// <summary>
    /// Add new user in repository.
    /// </summary>
    /// <param name="user">User.</param>
    public Guid AddNewUser(User user)
    {
      return this.users.Add(user);
    }

    /// <summary>
    /// Get all users from repository.
    /// </summary>
    /// <returns>Collection of users.</returns>
    public ICollection<User> GetUsers()
    {
      return this.users.GetList();
    }

    /// <summary>
    /// Get user from repository.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>User.</returns>
    public User GetUser(Guid id)
    {
      return this.users.GetItem(id);
    }

    /// <summary>
    /// Delete user from repository.
    /// </summary>
    /// <param name="user">User.</param>
    public void DeleteUser(User user)
    {
      this.users.Delete(user);
    }

    /// <summary>
    /// Delete user from repository by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    public void DeleteUser(Guid id)
    {
      this.users.Delete(this.users.GetItem(id));
    }
  }
}