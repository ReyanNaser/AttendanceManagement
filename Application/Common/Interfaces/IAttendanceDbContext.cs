using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IAttendanceDbContext
{
    DbSet<Domain.Entities.Employee> Employees { get; }
    DbSet<Domain.Entities.AttendanceRecord> AttendanceRecords { get; }
    DbSet<Domain.Entities.LeaveRequest> LeaveRequests { get; }
    DbSet<Domain.Entities.WorkFromHome> WorkFromHomes { get; }
    DbSet<Domain.Entities.Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
