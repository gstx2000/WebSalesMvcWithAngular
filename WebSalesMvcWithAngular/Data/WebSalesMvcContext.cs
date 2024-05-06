using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Models;

namespace WebSalesMvc.Data
{
    [Help("ProductSupplier é uma tabela auxiliar entre produtos e fornecedores, por isso foi definida" +
        "sem chave primária.")]

    public class WebSalesMvcContext : DbContext
    {
        public WebSalesMvcContext()
        {
        }
        public WebSalesMvcContext (DbContextOptions<WebSalesMvcContext> options)
            : base(options){}
        public DbSet<Department> Department { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<SalesRecord> SalesRecord { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<ProductSupplier> ProductSupplier { get; set; }
        public DbSet<Adress> Adress { get; set; } = default!;
        public DbSet<Supplier> Supplier { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesRecord>()
                .Property(s => s.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ProductSupplier>()
        .HasKey(ps => new { ps.ProductId, ps.SupplierId }); // Define composite primary key for junction table

            // Configure the many-to-many relationship between Product and Supplier
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Suppliers)
                .WithOne(ps => ps.Product)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithOne(ps => ps.Supplier)
                .HasForeignKey(ps => ps.SupplierId);

            modelBuilder.Entity<Adress>()
             .HasOne(a => a.Supplier)
             .WithMany(s => s.Adresses)
             .HasForeignKey(a => a.SupplierId);

            modelBuilder.Entity<Adress>()
               .HasOne(a => a.Customer)
               .WithMany(c => c.Adresses)
               .HasForeignKey(a => a.CustomerId);
        }
    }
}
