using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;

namespace MoviesApi.Models
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = 1,
                    Name = "User",
                },
                new Permission
                {
                    Id = 2,
                    Name = "Admin",
                });

        }
    }
}
