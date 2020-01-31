using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microservice.Whatevers.Data.Contexts
{
    public interface IDbContext
    {
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = default, CancellationToken cancellationToken = default);

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;
    }
}