using MediaControlApp.Domain.Models.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Infrastructure.DataAccess.MediaStore.Configuration
{
   
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> modelBuilder)
        {

            modelBuilder.HasKey(x => x.Id);
            modelBuilder.HasMany(x => x.Medias).WithOne(y => y.Author).HasForeignKey(y => y.AuthorId);

        }
    }
}
