using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {

        }
        public SalesContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Store> Stores { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationConection.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity
                .Property(p => p.Name)
                .IsUnicode(true);
                entity
                .Property(p => p.Description)
                .HasDefaultValue("No description");
            });
            modelBuilder.Entity<Customer>(entity =>
            {
                entity
                .Property(c => c.Name)
                .IsUnicode(true);

                entity
                .Property(c => c.Email)
                .IsUnicode(false);

                entity
                .Property(c => c.Email)
                .IsUnicode(false);

            });
            modelBuilder.Entity<Store>(entity =>
            {
                entity
                .Property(s => s.Name)
                .IsUnicode(true);
            });
            modelBuilder.Entity<Sale>(entity =>
            {
                entity
                .Property(s => s.Date)
                .HasDefaultValueSql("GETDATE()");
                        
            });


        }

    }
}
