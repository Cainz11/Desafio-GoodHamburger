using GoodHamburger.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.Lines);

        builder
            .HasMany(typeof(OrderLine), "_lines")
            .WithOne()
            .HasForeignKey(nameof(OrderLine.OrderId))
            .OnDelete(DeleteBehavior.Cascade);
    }
}
