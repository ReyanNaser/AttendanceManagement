using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.NotificationHub
{
    public class SignalRNotifier: IRealTimeNotifier
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRNotifier(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendAsync(Guid userId, Notification notification)
        {
            await _hub.Clients
                .Group(userId.ToString())
                .SendAsync("notificationReceived", notification);
        }
    }
}
