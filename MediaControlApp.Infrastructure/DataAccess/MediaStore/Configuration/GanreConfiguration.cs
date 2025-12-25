using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Configuration
{
    public class GanreConfiguration : IEntityTypeConfiguration<Ganre>
    {
        public void Configure(EntityTypeBuilder<Ganre> modelBuilder)
        {

            modelBuilder.HasKey(x => x.Id);
            modelBuilder.HasIndex(x => x.Name).IsUnique();
            modelBuilder.Property(x => x.Name).IsRequired();

        

            modelBuilder.HasOne(x=>x.MediaType).WithMany(y=>y.Ganres).HasForeignKey(x=>x.MediaTypeId);

            modelBuilder.HasMany(x=>x.Medias).WithOne(y=>y.Ganre).HasForeignKey(x=>x.GanreId);

        }
    }
}
