using Domain.Entities;
using Domain.Persistance;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitofWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AttendanceDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Employee, AttendanceDbContext>? _employees;
    private IRepository<AttendanceRecord, AttendanceDbContext>? _attendanceRecords;
    private IRepository<LeaveRequest, AttendanceDbContext>? _leaveRequests;
    private IRepository<WorkFromHome, AttendanceDbContext>? _workFromHomes;
    private IRepository<Manager, AttendanceDbContext>? _manager;

    public UnitOfWork(AttendanceDbContext context)
    {
        _context = context;
    }


    public IRepository<Employee, AttendanceDbContext> Employees =>
        _employees ??= new Repository<Employee, AttendanceDbContext>(_context);

    public IRepository<AttendanceRecord, AttendanceDbContext> AttendanceRecords =>
        _attendanceRecords ??= new Repository<AttendanceRecord, AttendanceDbContext>(_context);

    public IRepository<LeaveRequest, AttendanceDbContext> LeaveRequests =>
        _leaveRequests ??= new Repository<LeaveRequest, AttendanceDbContext>(_context);

    public IRepository<WorkFromHome, AttendanceDbContext> WorkFromHomes =>
        _workFromHomes ??= new Repository<WorkFromHome, AttendanceDbContext>(_context);

    public IRepository<Manager, AttendanceDbContext> Manager => 
        _manager ??= new Repository<Manager, AttendanceDbContext>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    


    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    
}