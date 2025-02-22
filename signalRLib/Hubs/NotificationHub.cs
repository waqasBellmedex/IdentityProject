using Domain.Common;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signalRLib.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ICurrentUserService _currentUserService;

        public NotificationHub(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override  Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, _currentUserService.UserId.ToString());

            if (!UserInfo.Connections.ContainsKey(Context.ConnectionId)) 
            {
                UserInfo.Connections.TryAdd(Context.ConnectionId, _currentUserService.UserId.ToString());
            }
            else
            {
                UserInfo.Connections[_currentUserService.UserId.ToString()] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
