using Microsoft.EntityFrameworkCore;
using AngaSystem.API.Models;

namespace AngaSystem.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}