using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace RoomApi.Controllers
{
  /// <summary>
  /// User controller. Allows work with users.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private UserService userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userService">User service</param>
    public UserController(UserService userService)
    {
      this.userService = userService;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>All users.</returns>
    [HttpGet]
    public IEnumerable<UserDTO> Get()
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
    public UserDTO Get(Guid id)
    {
      return new UserDTO(this.userService.GetUser(id));
    }

    /// <summary>
    /// Gets user by ConnectionID.
    /// </summary>
    /// <param name="id">User ID></param>
    /// <returns>User.</returns>
    [HttpGet("GetByConnectionId")]
    public UserDTO GetByConnectionId(string connectionId)
    {
      return new UserDTO(this.userService.GetUserByConnectionId(connectionId));
    }

    /// <summary>
    /// Changing user's name.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="newName">New name.</param>
    [HttpPut("{id}/ChangeName")]
    public void ChangeName(Guid id, string newName)
    {
      this.userService.GetUser(id).Name = newName;
    }
  }
}