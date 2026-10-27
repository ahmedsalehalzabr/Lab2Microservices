using Microsoft.EntityFrameworkCore;

namespace WarehouseService;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

    public DbSet<Item> Items => Set<Item>();
}