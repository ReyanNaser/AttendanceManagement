using Domain.Entities;
using Application.Common.Interfaces;
using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NotificationService
{
    public class NotificationService : INotificationService
    {
        
        private readonly IRealTimeNotifier _notifier;
        private readonly IAttendanceDbContext _db;
        public NotificationService(IAttendanceDbContext db, IRealTimeNotifier notifier)
        {
            _db = db;
            _notifier = notifier;
        }

        public async Task NotifyAsync(Guid userId,string title, string message, CancellationToken ct)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false
            };

            await _db.Notifications.AddAsync(notification, ct);
            await _db.SaveChangesAsync(ct);
            await _notifier.SendAsync(userId, notification);
        }
    }

}
