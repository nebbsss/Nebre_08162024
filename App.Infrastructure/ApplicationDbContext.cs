using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<CsvLogEntity> CsvLogs => Set<CsvLogEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
