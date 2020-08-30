using System;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.SignalR;

namespace RoomApi
{
  /// <summary>
  /// RoomHub class.
  /// </summary>
  public class RoomHub : Hub
  {
    private UserRepository users = new UserRepository();

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomHub"/> class.
    /// </summary>
    public RoomHub()
    {
    }

    /// <summary>
    /// When user connected to hub.
    /// </summary>
    /// <returns>Async task.</returns>
    public override async Task OnConnectedAsync()
    {
      await base.OnConnectedAsync();
    }

    /// <summary>
    /// When user disconnected.
    /// </summary>
    /// <param name="exception">Exception.</param>
    /// <returns>Async task.</returns>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
      await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// When user logining.
    /// </summary>
    /// <param name="userName">User's name</param>
    /// <returns>Async task.</returns>
    public async Task Login(string userName)
    {
      if (this.users.GetByConnectionID(Context.ConnectionId) == null)
      {
        this.users.Add(new User(userName, Context.ConnectionId));
        await Clients.Caller.SendAsync("onLogin");
      }
    }

    /// <summary>
    /// When user relogining.
    /// </summary>
    /// <param name="userId">User's ID</param>
    /// <returns>Async task.</returns>
    public async Task Relogin(Guid userId, Guid roomId)
    {
      if (this.users.GetItem(userId) != null)
      {
        if (this.users.GetItem(userId).ConnectionId != Context.ConnectionId)
        {
          this.users.GetItem(userId).ConnectionId = Context.ConnectionId;
          this.Groups.AddToGroupAsync(Context.ConnectionId, this.GetGroupKey(roomId)).Wait();
        }
        await Clients.Caller.SendAsync("onLogin");
      }
    }

    private string GetGroupKey(Guid roomId)
    {
      return $"room{roomId}";
    }
  }
}