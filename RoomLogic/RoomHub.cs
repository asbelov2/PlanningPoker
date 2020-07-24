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
    private IHubContext<RoomHub> context;
    private UserRepository users = new UserRepository();
    private RoundService roundService;
    private RoundRepository rounds = new RoundRepository();
    private RoomService roomService;
    private RoomRepository rooms = new RoomRepository();
    private DeckRepository decks = new DeckRepository();

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
      if (users.GetItem(Context.ConnectionId) == null)
      {
        users.Add(new User(userName, Context.ConnectionId));
      }
      await Clients.Caller.SendAsync("onLogin");
    }
  }
}