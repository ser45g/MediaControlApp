
using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore
{
    public class MediaDbContext : DbContext
    {

        public DbSet<Author> Authors { get; set; }

        public DbSet<Ganre> Ganres { get; set; }

        public DbSet<Media> Medias { get; set; }

        public DbSet<MediaType> MediaTypes { get; set; }
        public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
