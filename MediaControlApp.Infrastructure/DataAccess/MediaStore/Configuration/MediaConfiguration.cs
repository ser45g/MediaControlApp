using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Configuration
{
   public class MediaConfiguration : IEntityTypeConfiguration<Media>
   {
        public void Configure(EntityTypeBuilder<Media> modelBuilder)
        {
            modelBuilder.HasKey(x => x.Id);
            modelBuilder.HasIndex(x => x.Title).IsUnique();
            modelBuilder.Property(x=>x.Title).IsRequired();

            modelBuilder.HasOne(x => x.Author).WithMany(y => y.Medias).HasForeignKey(x=>x.AuthorId);
            modelBuilder.HasOne(x => x.Ganre).WithMany(y => y.Medias).HasForeignKey(x => x.GanreId);
            modelBuilder.OwnsOne(x => x.Rating, o =>
            {
                o.Property(x => x.Value);
                
            });
        }
    }
}
