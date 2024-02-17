using ChatRoom.Infrastracture.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatRoom.Infrastracture.Repositories
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>
		where TDbContext : DbContext
	{
		protected readonly DbContext _context;

		public UnitOfWork(TDbContext context)
		{
			_context = context;
		}

		public void UpdateState<TEntity>(TEntity entity, EntityState state)
		{
			_context.Entry(entity).State = state;
		}

		public void SetEntityStateModified<TEntiy, TProperty>(TEntiy entity, Expression<Func<TEntiy, TProperty>> propertyExpression) where TEntiy : class where TProperty : class
		{
			_context.Entry(entity).Reference(propertyExpression).IsModified = true;
		}

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			using var saveChangeTask = _context.SaveChangesAsync(cancellationToken);
			return await saveChangeTask;
		}

		public DbSet<T> Set<T>(string name = null) where T : class
		{
			return _context.Set<T>(name);
		}

		public void AddOrUpdateGraph<TEntiy>(TEntiy entity) where TEntiy : class
		{
			_context.ChangeTracker.TrackGraph(entity, e =>
			{
				var alreadyTrackedEntity = _context.ChangeTracker.Entries().FirstOrDefault(entry => entry.Entity.Equals(e.Entry.Entity));
				if (alreadyTrackedEntity != null)
				{
					return;
				}
				if (e.Entry.IsKeySet)
				{
					e.Entry.State = EntityState.Modified;
				}
				else
				{
					e.Entry.State = EntityState.Added;
				}
			});
		}

		public void Dispose()
		{
			_context?.Dispose();
		}
	}
}
