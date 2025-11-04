using Microsoft.EntityFrameworkCore;
using FluxoCaixa.Core.Models;

namespace FluxoCaixa.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}