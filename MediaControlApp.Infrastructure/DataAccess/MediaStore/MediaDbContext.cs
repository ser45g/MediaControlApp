
using MediaControlApp.Infrastructure.DataAccess.MediaStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore
{
    public class MediaDbContext : DbContext
    {

        public DbSet<Entities.Author> Authors { get; set; }

        public DbSet<Entities.Ganre> Ganres { get; set; }

        public DbSet<Entities.Media> Medias { get; set; }

        public DbSet<Entities.MediaType> MediaTypes { get; set; }
        public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options)
        {
            
        }

        protected MediaDbContext()
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
