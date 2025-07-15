using System.Linq.Expressions;
using Csb.YerindeDonusum.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class GenericRepositoryGeneralAsync<T, TContext> : IGenericRepositoryGeneralAsync<T> where T : class, new() where TContext : DbContext
{
    private readonly TContext _dbContext;

    public GenericRepositoryGeneralAsync(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbSet<T> Entity => _dbContext.Set<T>();

    public IQueryable<T> GetAllQueryable()
        => _dbContext.Set<T>();

    public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate)
        => _dbContext.Set<T>().Where(predicate);

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);

        return entity;
    }

    public Task DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    private async Task<int> SaveChangesBase(CancellationToken? cancellationToken)
    {
        if (cancellationToken == null)
            return await _dbContext.SaveChangesAsync();

        return await _dbContext.SaveChangesAsync(cancellationToken.Value);
    }

    public async Task<int> SaveChanges()
        => await SaveChangesBase(null);

    public async Task<int> SaveChanges(CancellationToken cancellationToken)
        => await SaveChangesBase(cancellationToken);

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes)
    {
        var query = Entity.AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        query = ApplyIncludes(query, includes);

        return query;
    }

    private static IQueryable<T> ApplyIncludes(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
    {
        if (includes != null)
        {
            foreach (var includeItem in includes)
            {
                query = query.Include(includeItem);
            }
        }

        return query;
    }
}