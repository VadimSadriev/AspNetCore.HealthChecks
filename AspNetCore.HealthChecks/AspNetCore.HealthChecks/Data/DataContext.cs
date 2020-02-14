using Microsoft.EntityFrameworkCore;

namespace AspNetCore.HealthChecks.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
