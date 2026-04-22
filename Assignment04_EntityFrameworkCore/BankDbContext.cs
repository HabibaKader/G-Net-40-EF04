using Assignment04_EntityFrameworkCore.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment04_EntityFrameworkCore
{
    public class BankDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AccountCustomer> AccountCustomers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=BankDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBranchManager(modelBuilder);
            ConfigureAccountRelations(modelBuilder);
            ConfigureAccountCustomer(modelBuilder);
            ConfigureTransactions(modelBuilder);
        }
        private void ConfigureBranchManager(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Manager)
                .WithOne(m => m.Branch)
                .HasForeignKey<Branch>(b => b.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        private void ConfigureAccountRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Branch)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        private void ConfigureAccountCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountCustomer>()
                .HasKey(ac => new { ac.AccountId, ac.CustomerId });

            modelBuilder.Entity<AccountCustomer>()
                .HasOne(ac => ac.Account)
                .WithMany(a => a.AccountCustomers)
                .HasForeignKey(ac => ac.AccountId);

            modelBuilder.Entity<AccountCustomer>()
                .HasOne(ac => ac.Customer)
                .WithMany(c => c.AccountCustomers)
                .HasForeignKey(ac => ac.CustomerId);
        }
        private void ConfigureTransactions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);
        }
    }
}
