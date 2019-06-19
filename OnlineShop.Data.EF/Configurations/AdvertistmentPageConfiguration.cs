using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Data.EF.Extensions;
using OnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data.EF.Configurations
{
    public class AdvertistmentPageConfiguration : DbEntityConfiguration<AdvertistmentPage>
    {
        public override void Configure(EntityTypeBuilder<AdvertistmentPage> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(20)
                .IsRequired().HasColumnType("varchar(20)");
        }
    }
}
