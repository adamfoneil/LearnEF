using LearnEF.Database;
using LearnEF.Database.Conventions;
using Microsoft.EntityFrameworkCore;

namespace LearnEF.Services
{
    internal class OrdersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LearnEF;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(nameof(Order.ProductId), nameof(Order.CustomerId));

            //modelBuilder.Entity<Customer>().HasMany<Order>().OnDelete

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var row in ChangeTracker.Entries<BaseTable>())
            {
                if (row.Entity.Id == 0) row.Entity.DateCreated = DateTime.Now;
                if (row.Entity.Id != 0) row.Entity.DateModified = DateTime.Now;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
