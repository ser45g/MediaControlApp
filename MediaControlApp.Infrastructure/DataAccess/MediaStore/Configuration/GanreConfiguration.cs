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
            modelBuilder.HasOne(x=>x.MediaType).WithMany(y=>y.Ganres).HasForeignKey(x=>x.MediaTypeId);

        }
    }
}
