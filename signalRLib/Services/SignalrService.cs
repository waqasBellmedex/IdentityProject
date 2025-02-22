using Domain.Interface;
using Domain.Model;
using Microsoft.AspNetCore.SignalR;
using signalRLib.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signalRLib.Services
{
    public class SignalrService : ISignalrService
    {
        public readonly IHubContext<NotificationHub> _notificationHub;
        public SignalrService(IHubContext<NotificationHub> notificationHub) { _notificationHub = notificationHub; }

        public async Task SendReload(SignalRRequestData<object> request)
        {
            if(!string.IsNullOrEmpty(request.UserId))
            {
                await _notificationHub.Clients.AllExcept(UserInfo.Connections[request.UserId]).SendAsync("SendReload", request);
            }
            else
            {
                await _notificationHub.Clients.All.SendAsync("SendRealod", request);
            }
        }
    }
}
