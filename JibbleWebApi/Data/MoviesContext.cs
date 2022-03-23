using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) :base(options)
        { }

        public DbSet<Favorites> Favorites { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
