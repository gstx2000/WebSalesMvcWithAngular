using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;

namespace WebSalesMvc.Data
{
    public class WebSalesMvcContext : DbContext
    {
        public WebSalesMvcContext()
        {
        }
        public WebSalesMvcContext (DbContextOptions<WebSalesMvcContext> options)
            : base(options)
        {
        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<SalesRecord> SalesRecord { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesRecord>()
                .Property(s => s.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
