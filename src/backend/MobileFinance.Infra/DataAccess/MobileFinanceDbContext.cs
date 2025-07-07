using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;

namespace MobileFinance.Infra.DataAccess;
public class MobileFinanceDbContext : DbContext
{
    public MobileFinanceDbContext(DbContextOptions<MobileFinanceDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MobileFinanceDbContext).Assembly);
}
