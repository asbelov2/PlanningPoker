using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace RoomApi.Controllers
{
  /// <summary>
  /// User controller.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private UserService userService = new UserService();

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>All users.</returns>
    [HttpGet]
    public ICollection<UserDTO> Get()
    {
      var users = new List<UserDTO>();
      foreach (var user in this.userService.GetUsers())
      {
        users.Add(new UserDTO(user));
      }

      return users;
    }

    /// <summary>
    /// Gets user by ID.
    /// </summary>
    /// <param name="id">User ID></param>
    /// <returns>User.</returns>
    [HttpGet("{id}")]
    public UserDTO Get(string id)
    {
      return new UserDTO(this.userService.GetUser(id));
    }

    /// <summary>
    /// Changing user's name.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="newName">New name.</param>
    [HttpPut("{id}/ChangeName")]
    public void ChangeName(string id, string newName)
    {
      this.userService.GetUser(id).Name = newName;
    }
  }
}