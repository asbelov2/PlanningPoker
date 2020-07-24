using Microsoft.AspNetCore.SignalR;
using RoomApi;

namespace PlanningPokerTests
{
  public class HubContextImplementation : IHubContext<RoomHub>
  {
    private HubContextImplementation()
    {
      this.Groups = GroupManager.GetGroupManager;
      this.Clients = HubClientsImplementation.GetHubClients;
    }

    public static IHubContext<RoomHub> GetContext { get; } = new HubContextImplementation();

    public IHubClients Clients { get; }

    public IGroupManager Groups { get; }
  }
}
