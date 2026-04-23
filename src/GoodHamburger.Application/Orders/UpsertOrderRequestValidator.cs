using FluentValidation;

namespace GoodHamburger.Application.Orders;

public sealed class UpsertOrderRequestValidator : AbstractValidator<UpsertOrderRequest>
{
    public UpsertOrderRequestValidator()
    {
        RuleFor(x => x.MenuItemIds)
            .NotNull()
            .WithMessage("A lista de itens é obrigatória.");

        RuleFor(x => x.MenuItemIds.Count)
            .LessThanOrEqualTo(3)
            .WithMessage("O pedido aceita no máximo três itens (um sanduíche, uma batata e um refrigerante).");
    }
}
