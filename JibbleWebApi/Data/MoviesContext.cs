using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) :base(options)
        { }

        public DbSet<Favorites> Favorites { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorites>(e => e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId));
            base.OnModelCreating(modelBuilder);
        }
    }
}
