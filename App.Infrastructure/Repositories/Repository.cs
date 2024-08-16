using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace App.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<Repository<TEntity>> logger;

    public Repository(IServiceProvider serviceProvider, ILogger<Repository<TEntity>> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    public async Task<TEntity?> Create(TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = GetDbContext();
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public async Task Delete(object id, CancellationToken cancellationToken = default)
    {
        var dbContext = GetDbContext();
        var existing = await dbContext.Set<TEntity>().FindAsync(id, cancellationToken).ConfigureAwait(false);
        if (existing is not null)
        {
            dbContext.Remove(existing);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task Delete(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var dbContext = GetDbContext();
        var items = await dbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!items.Any())
        {
            return;
        }

        foreach (var item in items)
        {
            dbContext.Remove(item);
        }

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public void Dispose() { }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default, params string[] includes)
    {
        var queryable = GetQueryable();
        foreach (string include in includes)
        {
            queryable = queryable.Include(include);
        }
        return await queryable.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<TEntity?> GetById(object id, CancellationToken cancellationToken = default)
    {
        return await GetDbContext().Set<TEntity>().FindAsync(id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await GetQueryable().Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, params string[] includes)
    {
        var queryable = GetQueryable();
        foreach (string include in includes)
        {
            queryable = queryable.Include(include);
        }
        return await queryable.Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default, params string[] includes)
    {
        var queryable = GetQueryable();

        foreach (string include in includes)
        {
            queryable = queryable.Include(include);
        }
        return queryable;
    }

    public async Task<TEntity?> Update(TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = GetDbContext();
        var existing = await dbContext.Set<TEntity>().FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
        if (existing is not null)
        {
            dbContext.Entry(existing).CurrentValues.SetValues(entity);

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            dbContext.Entry(existing).State = EntityState.Detached;

            return existing;
        }
        else
        {
            return entity;
        }
    }

    public async Task<TEntity?> Upsert(TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = GetDbContext();
        var existing = await dbContext.Set<TEntity>().FindAsync(entity.Id, cancellationToken).ConfigureAwait(false);
        if (existing is not null)
        {
            dbContext.Entry(existing).CurrentValues.SetValues(entity);

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            dbContext.Entry(existing).State = EntityState.Detached;

            return existing;
        }
        else
        {
            await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }
    }

    public DbContext GetDbContext()
    {
        return serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    private IQueryable<TEntity> GetQueryable()
    {
        return GetDbContext().Set<TEntity>().AsNoTracking();
    }
}
