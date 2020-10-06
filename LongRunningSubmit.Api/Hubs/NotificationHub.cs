using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LongRunningSubmit.Api.Hubs
{
    public class NotificationHub : Hub<INotificationHub>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendMessage(user, message);
        }
    }

    public interface INotificationHub
    {
        Task SendMessage(string user, string message);
    }
}