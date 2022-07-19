using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppContext : DbContext
{
    public virtual DbSet<Route> Routes { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    public virtual DbSet<Flight> Flights { get; set; }

    public AppContext()
    {
    }

    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}