using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatRoom.Infrastracture.Repositories.Interfaces
{
    public interface IUnitOfWork<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        DbSet<T> Set<T>(string name = null) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void AddOrUpdateGraph<TEntiy>(TEntiy entity) where TEntiy : class;
        void UpdateState<TEntity>(TEntity entity, EntityState state);
        void SetEntityStateModified<TEntiy, TProperty>(TEntiy entity, Expression<Func<TEntiy, TProperty>> propertyExpression) where TEntiy : class where TProperty : class;

    }
}
