using Microsoft.EntityFrameworkCore;

namespace Energia.WebSocket
{
    
    public class EnergiaDbContext : DbContext
    {
        public DbSet<EnergiaDados> Consumos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=energia_cosumo.db");
        }
    }
}
