using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Data.EF.Extensions;
using OnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Configurations
{
    public class AnnouncementConfiguration : DbEntityConfiguration<Announcement>
    {
        public override void Configure(EntityTypeBuilder<Announcement> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(50)
                .IsRequired().HasColumnType("varchar(128)");
        }
    }
}
