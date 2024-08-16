using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using App.Domain.Entities;

namespace App.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder.Property(e => e.Id).HasColumnName("UserId");
        builder.HasKey(e => e.UserId);
        builder.Ignore(e => e.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Weight).IsRequired();
        builder.Property(x => x.Height).IsRequired();
        builder.Property(x => x.BirthDate).IsRequired();
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.Bmi).IsRequired();
        builder.Property(x => x.DateCreated).IsRequired();
        builder.Property(x => x.DateUpdated).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
    }
}
