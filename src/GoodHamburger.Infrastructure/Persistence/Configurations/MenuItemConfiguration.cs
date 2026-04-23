using GoodHamburger.Domain.Menu;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Persistence.Configurations;

public sealed class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("menu_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.Role)
            .HasConversion<int>();

        builder.HasData(
            MenuItem.Create(MenuCatalog.XBurger, "X Burger", 5.00m, MenuItemRole.Sandwich),
            MenuItem.Create(MenuCatalog.XEgg, "X Egg", 4.50m, MenuItemRole.Sandwich),
            MenuItem.Create(MenuCatalog.XBacon, "X Bacon", 7.00m, MenuItemRole.Sandwich),
            MenuItem.Create(MenuCatalog.Fries, "Batata frita", 2.00m, MenuItemRole.Fries),
            MenuItem.Create(MenuCatalog.Soda, "Refrigerante", 2.50m, MenuItemRole.Soda));
    }
}
