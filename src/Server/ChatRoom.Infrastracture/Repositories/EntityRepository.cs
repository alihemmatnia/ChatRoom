using ChatRoom.Framework.Exceptions;
using ChatRoom.Infrastracture.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatRoom.Infrastracture.Repositories
{
    public abstract class EntityRepository<TEntity, TDbContext> : IEntityRepository<TEntity>, IDisposable
		where TEntity : class
		where TDbContext : DbContext
	{
		protected internal readonly IUnitOfWork<TDbContext> _context;
		protected internal readonly DbSet<TEntity> _dbSet;

		public EntityRepository(IUnitOfWork<TDbContext> context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
		}
		//public virtual async Task<TEntity> GetOneAsync(Guid id, CancellationToken cancellationToken)
		//{
		//    return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
		//}

		public Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			return _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
		}

		public virtual async Task DeleteByIdAsync(long id, CancellationToken cancellationToken)
		{
			var entity = await GetOneAsync(id, cancellationToken);
			if (entity != null)
				_dbSet.Remove(entity);

			throw new ChatRoomRepositoryNotFound(typeof(TEntity).Name);
		}
		public virtual async Task<TEntity?> GetOneAsync(long id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
		}

		public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _dbSet.ToListAsync(cancellationToken);
		}

		public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
		}



		public virtual Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			return _dbSet.AnyAsync(predicate, cancellationToken);
		}

		public virtual TEntity Add(TEntity entity)
		{
			_dbSet.Add(entity);
			return entity;
		}

		public virtual bool AddRange(params TEntity[] entities)
		{
			_dbSet.AddRange(entities);
			return true;
		}

		public virtual TEntity Attach(TEntity entity)
		{
			var entry = _dbSet.Attach(entity);
			entry.State = EntityState.Added;
			return entity;
		}

		public virtual TEntity Update(TEntity entity)
		{
			_dbSet.Update(entity);
			return entity;
		}

		public TEntity Modify(TEntity entity)
		{
			var entry = _dbSet.Attach(entity);
			foreach (var property in entry.Properties)
			{
				var original = property.OriginalValue;
				var current = property.CurrentValue;

				if (ReferenceEquals(original, current))
				{
					continue;
				}

				if (original == null)
				{
					property.IsModified = true;
					continue;
				}

				var propertyIsModified = !original.Equals(current);
				property.IsModified = propertyIsModified;
			}
			return entity;
		}

		public virtual bool UpdateRange(params TEntity[] entities)
		{
			_dbSet.UpdateRange(entities);
			return true;
		}

		public abstract Task<TEntity> CreateOrUpdateAsync(TEntity entity);


		public virtual async Task Clear(CancellationToken cancellationToken = default)
		{
			var allEntities = await _dbSet.ToListAsync(cancellationToken);
			_dbSet.RemoveRange(allEntities);
		}


		public virtual async Task DeleteAsync(TEntity entity)
		{
			await Task.FromResult(_dbSet.Remove(entity));
		}

		public virtual void DeleteRange(TEntity[] entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var effected = await _context.SaveChangesAsync(cancellationToken);
			return effected;
		}

		public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
		{
			var countTask = await _dbSet.CountAsync(cancellationToken);
			return countTask;
		}

		protected async Task RemoveManyToManyRelationship(string joinEntityName, string ownerIdKey, string ownedIdKey, long ownerEntityId, List<long> idsToIgnore)
		{
			DbSet<Dictionary<string, object>> dbset = _context.Set<Dictionary<string, object>>(joinEntityName);

			var manyToManyData = await dbset
				.Where(joinPropertyBag => joinPropertyBag[ownerIdKey].Equals(ownerEntityId))
				.ToListAsync();

			var filteredManyToManyData = manyToManyData
				.Where(joinPropertyBag => !idsToIgnore.Any(idToIgnore => joinPropertyBag[ownedIdKey].Equals(idToIgnore)))
				.ToList();

			dbset.RemoveRange(filteredManyToManyData);
		}

		public void Dispose()
		{
			_context?.Dispose();
		}

	}
}
