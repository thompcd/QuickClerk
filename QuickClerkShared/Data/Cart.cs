namespace QuickClerkShared.Data
{
    public class Cart
    {
        public int Id { get; set; }
        public double Subtotal { get; set; }
        public double Total { get; set; }
        public ICollection<Cart> Carts { get; set;}
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
