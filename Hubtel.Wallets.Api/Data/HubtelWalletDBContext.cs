using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Api.Data
{
    public class HubtelWalletDBContext : DbContext
    {
        public HubtelWalletDBContext(DbContextOptions<HubtelWalletDBContext> options)
            : base(options)
        {
        }
        public DbSet<AccountScheme> AccountScheme { get; set; }
        public DbSet<AccountType> AccountType { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<WalletLimit> WalletLimit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-RR8EHIV;Initial Catalog=HubtelWalletDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WalletLimit>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
