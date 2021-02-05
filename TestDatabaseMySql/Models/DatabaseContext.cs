using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLog;
using System;

namespace TestDatabaseMySql.Models
{
    public class DatabaseContext : DbContext
    {
        private readonly Logger _logger;
        private string _connectionString;
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DatabaseContext(Logger logger)
        {
            _logger = logger;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                _connectionString = new Settings(_logger).GetConnectionString();
            }
            catch
            {
                System.Windows.Application.Current.Shutdown(1);
            }
            optionsBuilder.UseMySql(_connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(DepartmentConfigure);
            modelBuilder.Entity<Employee>(EmployeeConfigure);
            modelBuilder.Entity<Order>(OrderConfigure);
            modelBuilder.Entity<Product>(ProductConfigure);
        }

        public void DepartmentConfigure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Title).IsRequired();
            builder.Property(d => d.Title).HasMaxLength(500);
            builder.HasOne(d => d.Manager).WithMany(e => e.Departments).HasForeignKey(d => d.ManagerId).OnDelete(DeleteBehavior.SetNull);
        }
        public void EmployeeConfigure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Gender).IsRequired();
            builder.Property(e => e.FirstName).IsRequired();
            builder.Property(e => e.BirthDate).IsRequired();
            builder.Property(e => e.FirstName).HasMaxLength(200);
            builder.Property(e => e.LastName).HasMaxLength(200);
            builder.Property(e => e.MiddleName).HasMaxLength(200);
            builder.HasOne(e => e.Department).WithMany(d => d.Managers).HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.SetNull);
        }

        public void OrderConfigure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.CreationDate).HasColumnType("datetime").HasDefaultValueSql("current_timestamp()");
            builder.Property(o => o.CustomerName).IsRequired();
            builder.Property(o => o.CustomerName).HasMaxLength(500);
            builder.HasOne(o => o.Creator).WithMany(e => e.Orders).HasForeignKey(o => o.CreatorId).OnDelete(DeleteBehavior.SetNull);            
        }

        public void ProductConfigure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.Price).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.Name).HasMaxLength(500);
            builder.HasOne(p => p.Order).WithMany(o => o.Products).HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade);
        }

    }
}
