using Csb.YerindeDonusum.Domain.Common;
using EFCore.BulkExtensions;
using System.Linq.Expressions;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IGenericRepositoryAsync<T> where T : BaseEntity
{
    //Task<T> GetByIdAsync(Guid id);

    //Task<T> GetByIdAsync(Guid id, bool isAsNoTracking);

    IQueryable<T> GetAllQueryable();

    IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate);

    IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);
    //T FirstOrDefault(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);
    int Count(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);

    IEnumerable<T> GetWhereEnumerable(Func<T, bool> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);

    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> list);
    void BulkInsert(IEnumerable<T> entity, BulkConfig? bulkConfig = null);

    T Update(T entity);
    IEnumerable<T> UpdateRange(IEnumerable<T> list);
    //Task DeleteAsync(T entity);

    //Task DeleteByIdAsync(Guid id);

    //void Delete(T entity);

    Task<int> SaveChanges();

    Task<int> SaveChanges(CancellationToken cancellationToken);

    Task BulkUpdate(IEnumerable<T> entity, CancellationToken cancellationToken, BulkConfig? bulkConfig = null);

    Task SaveChangesBulk(CancellationToken cancellationToken);
}