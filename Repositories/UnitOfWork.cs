using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using FastDeliveryApi.Data;

namespace FastDeliveryApi.Repositories.Interfaces;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly FastDeliveryDbContext _dbContext;

    public UnitOfWork(FastDeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntites();
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntites()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if(entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOnUtc)
                           .CurrentValue = DateTime.UtcNow;
            }

            if(entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOnUtc)
                           .CurrentValue = DateTime.UtcNow;
            }
        }
    }
}