using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<Parking> Parking { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
#if TESTE
        SeedTest.OnModelCreating(builder);
#else
#endif
        base.OnModelCreating(builder);
    }
}
