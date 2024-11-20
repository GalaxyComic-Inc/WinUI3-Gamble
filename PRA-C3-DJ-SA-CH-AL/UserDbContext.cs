using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace PRA_C3_DJ_SA_CH_AL
{
    public class UserDbContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +                                               // Server name
                "port=3306;" +                                                      // Server port
                "user=root;" +                                                      // Username
                "password=;" +                                                      // Password
                "database=django_test;"                                             // Database name
                , Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql") // Version
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
