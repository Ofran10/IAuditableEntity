using FastDeliveryApi.Data.Configurations;
using FastDeliveryApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace FastDeliveryApi.Data;

public class FastDeliveryDbContext : DbContext
{
    public FastDeliveryDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}