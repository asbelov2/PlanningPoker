using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace PlanningPokerTests
{
  public class HubClientsImplementation : IHubClients
  {
    private HubClientsImplementation()
    {
      this.All = new ClientProxy();
    }

    public static IHubClients GetHubClients { get; } = new HubClientsImplementation();

    public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds)
    {
      return new ClientProxy();
    }

    public IClientProxy Client(string connectionId)
    {
      return new ClientProxy();
    }

    public IClientProxy Clients(IReadOnlyList<string> connectionIds)
    {
      return new ClientProxy();
    }

    public IClientProxy Group(string groupName)
    {
      return new ClientProxy();
    }

    public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
    {
      return new ClientProxy();
    }

    public IClientProxy Groups(IReadOnlyList<string> groupNames)
    {
      return new ClientProxy();
    }

    public IClientProxy User(string userId)
    {
      return new ClientProxy();
    }

    public IClientProxy Users(IReadOnlyList<string> userIds)
    {
      return new ClientProxy();
    }

    public IClientProxy All { get; }
  }
}
