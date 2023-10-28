using DesafioBalta.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBalta.Data
{
    public sealed class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<IBGE> IBGEs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=DesafioBalta;User ID=SA;Password=41Gft.26@98k;Encrypt=false");
    }
}