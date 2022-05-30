using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApp
{
    public class AutomaticMigrations
    {
        private readonly MoviesDbContext _dbContext;

        public AutomaticMigrations(MoviesDbContext moviesDbContext)
        {
            _dbContext = moviesDbContext;
        }

        public void GetMigrations()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }
            }
        }

    }
}
