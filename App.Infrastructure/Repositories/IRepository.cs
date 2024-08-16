using App.Domain.Entities;
using System.Linq.Expressions;

namespace App.Infrastructure.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : class, IEntity
{
    Task<TEntity?> GetById(object id, CancellationToken cancellationToken = default);
    Task<TEntity?> Create(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> Update(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> Upsert(TEntity entity, CancellationToken cancellationToken = default);
    Task Delete(object id, CancellationToken cancellationToken = default);
    Task Delete(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default, params string[] includes);
    Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, params string[] includes);
    IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default, params string[] includes);
}