using Domain.Entities;
using Domain.Persistance;
using Infrastructure.Repository;

namespace Infrastructure.UnitofWork;

public interface IUnitOfWork : IDisposable
{
    IRepository<Employee, AttendanceDbContext> Employees { get; }
    IRepository<AttendanceRecord, AttendanceDbContext> AttendanceRecords { get; }
    IRepository<LeaveRequest, AttendanceDbContext> LeaveRequests { get; }
    IRepository<WorkFromHome, AttendanceDbContext> WorkFromHomes { get; }
    IRepository<Manager, AttendanceDbContext> Manager { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}