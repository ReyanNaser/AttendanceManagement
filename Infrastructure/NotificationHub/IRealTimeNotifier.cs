using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.NotificationHub
{
    public interface IRealTimeNotifier
    {
        Task SendAsync(Guid userId, Notification notification);
    }
}
