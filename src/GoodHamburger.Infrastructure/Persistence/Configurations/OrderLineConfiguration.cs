using GoodHamburger.Domain.Menu;
using GoodHamburger.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Persistence.Configurations;

public sealed class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable("order_lines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.MenuItemId).IsRequired();

        builder
            .HasOne<MenuItem>()
            .WithMany()
            .HasForeignKey(x => x.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OrderId, x.MenuItemId }).IsUnique();
    }
}
