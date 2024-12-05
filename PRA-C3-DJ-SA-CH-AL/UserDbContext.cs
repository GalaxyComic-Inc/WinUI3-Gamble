using Microsoft.EntityFrameworkCore;
using PRA_C3_DJ_SA_CH_AL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL
{
    public class UserDbContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Bets> Bets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +                                               // Server name
                "port=3306;" +                                                      // Server port
                "user=root;" +                                                      // Username
                "password=;" +                                                      // Password
                "database=PRA-C3-DJ-SA-CH-AL;"                                      // Database name
                , Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql") // Version
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configurations (if needed)
        }

        public async Task<List<Bets>> GetUserBetsAsync(int userId)
        {
            return await Bets
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
