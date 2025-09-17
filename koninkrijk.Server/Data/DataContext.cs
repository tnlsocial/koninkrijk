using koninkrijk.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace koninkrijk.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Province> Provinces { get; set; } = default!;
        public DbSet<Log> Logs { get; set; } = default!;
        public DbSet<PlayerProvince> PlayerProvince { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "Groningen", Description = "The province of Groningen", ProvinceSize = 4 },
                new Province { Id = 2, Name = "Fryslân", Description = "The province of Fryslân", ProvinceSize = 5 },
                new Province { Id = 3, Name = "Drenthe", Description = "The province of Drenthe", ProvinceSize = 4 },
                new Province { Id = 4, Name = "Overijssel", Description = "The province of Overijssel", ProvinceSize = 6 },
                new Province { Id = 5, Name = "Flevoland", Description = "The province of Flevoland", ProvinceSize = 3 },
                new Province { Id = 6, Name = "Gelderland", Description = "The province of Gelderland", ProvinceSize = 9 },
                new Province { Id = 7, Name = "Utrecht", Description = "The province of Utrecht", ProvinceSize = 2 },
                new Province { Id = 8, Name = "Noord-Holland", Description = "The province of Noord-Holland", ProvinceSize = 6 },
                new Province { Id = 9, Name = "Zuid-Holland", Description = "The province of Zuid-Holland", ProvinceSize = 7 },
                new Province { Id = 10, Name = "Zeeland", Description = "The province of Zeeland", ProvinceSize = 3 },
                new Province { Id = 11, Name = "Noord-Brabant", Description = "The province of Noord-Brabant", ProvinceSize = 8 },
                new Province { Id = 12, Name = "Limburg", Description = "The province of Limburg", ProvinceSize = 5 }
            );
        }
    }
}
