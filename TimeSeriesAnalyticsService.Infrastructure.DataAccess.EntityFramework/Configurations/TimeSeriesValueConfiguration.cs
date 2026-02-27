using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Entities;

namespace TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Configurations;

public sealed class TimeSeriesValueConfiguration : IEntityTypeConfiguration<TimeSeriesValueEntity>
{
    public void Configure(EntityTypeBuilder<TimeSeriesValueEntity> builder)
    {
        builder.ToTable("Values");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.FileName)
            .HasColumnName("FileName")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Date)
            .HasColumnName("Date")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(x => x.ExecutionTimeSeconds)
            .HasColumnName("ExecutionTimeSeconds")
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnName("Value")
            .HasColumnType("double precision")
            .IsRequired();
    }
}