using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class OrederConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(p => p.SubTotal).HasColumnType("decimal(8,2)");
            builder.OwnsOne(p => p.ShippingAddress);
            builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(50);
        }
    }
}
