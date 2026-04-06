using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AngaSystem.API.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseSqlServer(
                "Server=RENATO;Database=AngaSystem;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new DataContext(optionsBuilder.Options);
        }
    }
}