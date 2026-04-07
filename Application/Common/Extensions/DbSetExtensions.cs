using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Extensions;

public static class DbSetExtensions
{
    public static Task<List<T>> GetMany<T>(this DbSet<T> dbSet, Expression<System.Func<T, bool>> predicate, CancellationToken ct = default) where T : class
    {
        return dbSet.Where(predicate).AsNoTracking().ToListAsync(ct);
    }

    public static Task<List<T>> GetManyTracking<T>(this DbSet<T> dbSet, Expression<System.Func<T, bool>> predicate, CancellationToken ct = default) where T : class
    {
        return dbSet.Where(predicate).AsTracking().ToListAsync(ct);
    }

    public static ValueTask<T?> GetByIdAsync<T>(this DbSet<T> dbSet, object id, CancellationToken ct = default) where T : class
    {
        return dbSet.FindAsync(new object[] { id }, ct);
    }
}
