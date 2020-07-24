using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PlanningPokerTests
{
  public class ClientProxy : IClientProxy
  {
    public Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = new CancellationToken())
    {
      return Task.Run(() =>
      {
        ServiceTests.InvokedMethod = method;
        ServiceTests.Args = args;
      });
    }
  }
}
