using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Entities;

namespace TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Configurations;

public sealed class TimeSeriesResultConfiguration : IEntityTypeConfiguration<TimeSeriesResultEntity>
{
    public void Configure(EntityTypeBuilder<TimeSeriesResultEntity> builder)
    {
        builder.ToTable("Results");

        builder.HasKey(x => x.FileName);

        builder.Property(x => x.FileName)
            .HasColumnName("FileName")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.DeltaSeconds)
            .HasColumnName("DeltaSeconds")
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(x => x.FirstStart)
            .HasColumnName("FirstStart")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(x => x.AverageExecutionTimeSeconds)
            .HasColumnName("AverageExecutionTimeSeconds")
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(x => x.AverageValue)
            .HasColumnName("AverageValue")
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(x => x.MedianValue)
            .HasColumnName("MedianValue")
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(x => x.MaxValue)
            .HasColumnName("MaxValue")
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(x => x.MinValue)
            .HasColumnName("MinValue")
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(x => x.RowCount)
            .HasColumnName("RowCount")
            .IsRequired();

        builder.Property(x => x.UploadedAt)
            .HasColumnName("UploadedAt")
            .HasColumnType("timestamptz")
            .IsRequired();
    }
}