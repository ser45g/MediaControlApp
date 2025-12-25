using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Configuration
{
    public class MediaTypeConfiguration : IEntityTypeConfiguration<MediaType>
    {
        public void Configure(EntityTypeBuilder<MediaType> modelBuilder)
        {

            modelBuilder.HasKey(x => x.Id);
            modelBuilder.HasIndex(x=>x.Name).IsUnique();
            modelBuilder.Property(x => x.Name).IsRequired();

            modelBuilder.HasMany(x=>x.Ganres).WithOne(x => x.MediaType).HasForeignKey(x => x.MediaTypeId);

        }
    }
}
