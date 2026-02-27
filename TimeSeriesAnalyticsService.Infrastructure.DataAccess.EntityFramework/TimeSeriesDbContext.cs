using Microsoft.EntityFrameworkCore;
using TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Entities;

namespace TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework;

public sealed class TimeSeriesDbContext : DbContext
{
    public TimeSeriesDbContext(DbContextOptions<TimeSeriesDbContext> options) : base(options) { }

    public DbSet<TimeSeriesValueEntity> Values => Set<TimeSeriesValueEntity>();
    public DbSet<TimeSeriesResultEntity> Results => Set<TimeSeriesResultEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimeSeriesDbContext).Assembly);
    }
}