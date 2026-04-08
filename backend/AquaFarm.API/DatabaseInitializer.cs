using AquaFarm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaFarm.API
{
    public class DatabaseInitializer
    {
        private readonly AquaFarmDbContext _dbContext;

        public DatabaseInitializer(AquaFarmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            // Apply pending migrations and ensure database exists
            if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }
    }
}
