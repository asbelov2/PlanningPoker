using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PlanningPokerTests
{  
  public class GroupManager : IGroupManager
  {
    private Dictionary<string, ISet<string>> groupsConnections;

    private GroupManager()
    {
      this.groupsConnections = new Dictionary<string, ISet<string>>();
    }

    /// <summary>
    /// Gets group manager
    /// </summary>
    public static GroupManager GetGroupManager { get; } = new GroupManager();

    /// <summary>
    /// Adding user to group
    /// </summary>
    /// <param name="connectionId">User id</param>
    /// <param name="groupName">Group name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async task</returns>
    public Task AddToGroupAsync(
      string connectionId,
      string groupName,
      CancellationToken cancellationToken = default(CancellationToken))
    {
      return Task.Run(() =>
      {
        if (!this.groupsConnections.ContainsKey(groupName))
        {
          this.groupsConnections.Add(groupName, new HashSet<string>());
        }

        ISet<string> connections = this.groupsConnections.GetValueOrDefault(groupName);
        if (!connections.Contains(connectionId))
        {
          connections.Add(connectionId);
        }
      });
    }

    public Task RemoveFromGroupAsync(string connectionId, string groupName,
        CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        if (groupsConnections.ContainsKey(groupName))
        {
          this.groupsConnections[groupName].Remove(connectionId);
        }
      });
    }
  }
}