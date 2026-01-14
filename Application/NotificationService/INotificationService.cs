using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NotificationService
{
    public interface INotificationService
    {
        Task NotifyAsync(
            Guid userId,
            string title,
            string message,
            CancellationToken ct
        );
    }
}
