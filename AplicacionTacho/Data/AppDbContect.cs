using AplicacionTacho.Models;
using Microsoft.EntityFrameworkCore;

namespace AplicacionTacho.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppInfo> AppInfos { get; set; }
        public DbSet<AppComment> AppComments { get; set; }
    }
}
