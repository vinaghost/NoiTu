using BlazorApp.Enitites;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}