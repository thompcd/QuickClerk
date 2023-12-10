namespace QuickClerkShared.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public double Price { get; set; }
        public int QuantityInStock { get; set; }
        public ICollection<Product> Products { get; set; }
    }
    //public class CashRegisterContextDesignTimeFactory : IDesignTimeDbContextFactory<CashRegisterContext>
    //{
    //    public CashRegisterContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<CashRegisterContext>();
    //        optionsBuilder.UseSqlite("Data Source=data.db");

    //        return new BudgetContext(WeakReferenceMessenger.Default, optionsBuilder.Options);
    //    }
    //}
}
