using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inferastructure.DB.config
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.Rate).HasDefaultValue(0);
            builder.Property(x=>x.NumberInStock).HasDefaultValue(0).IsRequired();
            builder.HasOne(x => x.category).WithMany().HasForeignKey(p =>p.categoryID);
        }
    }
}
