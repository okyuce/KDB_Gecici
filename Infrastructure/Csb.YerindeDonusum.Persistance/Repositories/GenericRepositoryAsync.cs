using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Common;
using Csb.YerindeDonusum.Persistance.Context;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public GenericRepositoryAsync(ApplicationDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public DbSet<T> Entity => _dbContext.Set<T>();

    //public async Task<T> GetByIdAsync(Guid id) =>
    //    (await GetByIdAsync(id, false))!;

    //public async Task<T> GetByIdAsync(Guid id, bool isAsNoTracking)
    //{
    //    if (isAsNoTracking)
    //        return (await _dbContext.Set<T>().Where(p => p.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync())!;

    //    return (await _dbContext.Set<T>().Where(p => p.Id.Equals(id)).FirstOrDefaultAsync())!;
    //}

    public IQueryable<T> GetAllQueryable()
        => _dbContext.Set<T>();

    public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate)
    {
        var result = (IQueryable<T>)_dbContext.Set<T>();
        if (predicate is not null) result = result.Where(predicate);
        return result;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }
    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> list)
    {
        await _dbContext.Set<T>().AddRangeAsync(list);
        return list;
    }
    public T Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        return entity;
    }
    public IEnumerable<T> UpdateRange(IEnumerable<T> list)
    {
        _dbContext.Set<T>().UpdateRange(list);
        return list;
    }

    //public void Delete(T entity)
    //{
    //    entity.IsDeleted = true;
    //}

    //public Task DeleteByIdAsync(Guid id)
    //{
    //    throw new NotImplementedException();
    //}

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

    public async Task BulkUpdate(IEnumerable<T> entity, CancellationToken cancellationToken, BulkConfig? bulkConfig = null)
    {
        string oldConnectionString = _dbContext.Database.GetDbConnection().ConnectionString;

        _dbContext.Database.SetConnectionString(_configuration.GetConnectionString("YerindeDonusumDevUser"));

        await _dbContext.BulkUpdateAsync(entity, cancellationToken: cancellationToken, bulkConfig: bulkConfig);

        _dbContext.Database.SetConnectionString(oldConnectionString);
    }
    public void BulkInsert(IEnumerable<T> entity, BulkConfig? bulkConfig = null)
    {
        string oldConnectionString = _dbContext.Database.GetDbConnection().ConnectionString;

        _dbContext.Database.SetConnectionString(_configuration.GetConnectionString("YerindeDonusumDevUser"));

        _dbContext.BulkInsert(entity, bulkConfig: bulkConfig);

        _dbContext.Database.SetConnectionString(oldConnectionString);
    }

    public async Task SaveChangesBulk(CancellationToken cancellationToken)
    {
        string oldConnectionString = _dbContext.Database.GetDbConnection().ConnectionString;

        _dbContext.Database.SetConnectionString(_configuration.GetConnectionString("YerindeDonusumDevUser"));

        await _dbContext.BulkSaveChangesAsync(cancellationToken: cancellationToken);

        _dbContext.Database.SetConnectionString(oldConnectionString);
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes)
    {
        var query = Entity.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        query = ApplyIncludes(query, includes);

        return query;
    }

    //public T FirstOrDefault(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes)
    //{
    //    var query = Entity.AsQueryable();
    //    if (asNoTracking)
    //        query = query.AsNoTracking();
    //    if (predicate != null)
    //        query = query.Where(predicate);
    //    query = ApplyIncludes(query, includes);

    //    return query.FirstOrDefault();
    //}

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

    // Mehmet Sümer
    // 20.07.2023
    // veriyi filtrelemek için static metodlar yazdým lakin queryable db' de filtrelediðinden hata verdi,
    // Ienumerable döndürmem gerekti.
    public IEnumerable<T> GetWhereEnumerable(Func<T, bool> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes)
    {
        var query = Entity.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        query = ApplyIncludes(query, includes);

        var asEnum = query.AsEnumerable();

        if (predicate != null)
            asEnum = asEnum.Where(predicate);

        return asEnum;
    }

    public int Count(Expression<Func<T, bool>> predicate, bool asNoTracking = false, params Expression<Func<T, object>>[] includes)
    {
        var query = Entity.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        query = ApplyIncludes(query, includes);

        return query.Count();
    }

}