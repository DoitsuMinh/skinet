using Core.Enitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insfrastructure.Config;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
  public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
  {
    builder.Property(d => d.Price).HasColumnType("REAL");
  }
}