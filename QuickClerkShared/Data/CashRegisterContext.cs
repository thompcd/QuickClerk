using Microsoft.EntityFrameworkCore;

namespace QuickClerkShared.Data
{
    public class CashRegisterContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=QuickClerk.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
