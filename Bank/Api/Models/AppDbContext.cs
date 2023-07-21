using Bank.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Bank.Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<SymbolAc> SymbolAcs { get; set; }  


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>()
       .HasKey(c => c.UserId);

            modelBuilder.Entity<Wallet>()
                .HasKey(c => c.WalletId);

            modelBuilder.Entity<SymbolAc>()
                .HasKey(c => c.SymbolId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId);

            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.SymbolAcs)
                .WithOne(s => s.Wallet)
                .HasForeignKey(s => s.WalletId);

            modelBuilder.Entity<Wallet>().HasData(new Wallet
            {
                WalletId = 1,
                UserId = 1, 
                SymbolId = null,
                WalletName = "Investimentos",
            });

            
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = 1,
                FirstName = "Lucas",
                LastName = "asdsa",
                Email = "teste@gadsakm.com",
                JoinedDate = new DateTime(2023, 07, 19),
                UPassword = "ConfirmPassword",
                ConfirmPassword = "ConfirmPassword",
            });

            modelBuilder.Entity<SymbolAc>().HasData(
                new List<SymbolAc>
                {
            new SymbolAc
            {
                SymbolId = 1,
                WalletId = 1,
                SymbolName = "AAPL",
            },
            new SymbolAc
            {
                SymbolId = 2,
                WalletId = 1,
                SymbolName = "GOOGL",
            },
                });
        }
    }
}
