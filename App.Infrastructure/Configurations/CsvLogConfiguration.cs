using App.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Configurations;

public class CsvLogConfiguration : IEntityTypeConfiguration<CsvLogEntity>
{
    public void Configure(EntityTypeBuilder<CsvLogEntity> builder)
    {
        builder.ToTable("CsvLogs");
        builder.Property(e => e.Id).HasColumnName("CsvLogId");
        builder.HasKey(e => e.CsvLogId);
        builder.Ignore(e => e.Id);

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.RecordsProcessed).IsRequired();
        builder.Property(x => x.Duration).IsRequired();
        builder.Property(x => x.FileSize).IsRequired();
        builder.Property(x => x.DateCreated).IsRequired();
        builder.Property(x => x.DateUpdated).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
    }
}
