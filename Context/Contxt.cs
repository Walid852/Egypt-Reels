using JwtApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtApp.Context
{
    public class Contxt:DbContext
    {
        public Contxt(DbContextOptions<Contxt> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Likes>().HasKey(table => new {
                table.ReelId,
                table.username
            });
            builder.Entity<Share>().HasKey(table => new {
                table.ReelId,
                table.username
            });
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Reel> Reels { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<PropertiesOfReels> PropertiesOfReels { get; set; }
        public DbSet<Videos> videos { get; set; }


    }
}
