using ImtahanElnur.Models;
using Microsoft.EntityFrameworkCore;

namespace ImtahanElnur.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Profession> Professions { get; set; } = null!;
        public DbSet<Portfolio> Portfolios { get; set; } = null!;
    }
}
