﻿using Core.Enitities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insfrastructure.Config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(x => x.ItemOrdered, o => o.WithOwner());
            builder.Property(x => x.Price).HasColumnType("REAL");
        }
    }
}
