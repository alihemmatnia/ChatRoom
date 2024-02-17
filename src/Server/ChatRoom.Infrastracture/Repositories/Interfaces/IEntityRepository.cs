using System.Linq.Expressions;

namespace ChatRoom.Infrastracture.Repositories.Interfaces
{
    public interface IEntityRepository<TEntity>
    {
        Task<TEntity?> GetOneAsync(long id, CancellationToken cancellationToken = default);
        Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<TEntity> CreateOrUpdateAsync(TEntity entity);
        Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default);
        ///// Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAsync(TEntity entity);
        void DeleteRange(TEntity[] entities);
        Task Clear(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        TEntity Add(TEntity entity);
        bool AddRange(params TEntity[] entities);
        TEntity Attach(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Modify(TEntity entity);
        bool UpdateRange(params TEntity[] entities);
    }
}
