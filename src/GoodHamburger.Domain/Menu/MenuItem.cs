namespace GoodHamburger.Domain.Menu;

public sealed class MenuItem
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public MenuItemRole Role { get; private set; }

    private MenuItem()
    {
    }

    public static MenuItem Create(Guid id, string name, decimal unitPrice, MenuItemRole role)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Nome do item é obrigatório.", nameof(name));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Preço não pode ser negativo.");
        }

        return new MenuItem
        {
            Id = id,
            Name = name.Trim(),
            UnitPrice = unitPrice,
            Role = role
        };
    }
}
