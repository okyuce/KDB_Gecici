using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IGenericRepositoryGeneralAsync<T> where T : class, new()
{
    IQueryable<T> GetAllQueryable();

    IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate);

    IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);

    Task<T> AddAsync(T entity);

    Task DeleteAsync(T entity);

    Task DeleteByIdAsync(Guid id);

    Task<int> SaveChanges();

    Task<int> SaveChanges(CancellationToken cancellationToken);
}