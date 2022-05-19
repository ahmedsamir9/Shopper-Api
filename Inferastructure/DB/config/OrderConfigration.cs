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
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.OrderDate).IsRequired();
           
            builder.Property(x=>x.UserEmail).IsRequired();
            builder.OwnsOne(o => o.ShippedAddress, a =>
            {
                a.WithOwner();
            });
            //builder.HasMany(e => e.OrderItems).WithOne(o => o.Order);
        }
    }
}
