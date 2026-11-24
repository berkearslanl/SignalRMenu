using FluentValidation;
using SignalR.DtoLayer.Dtos.BasketDtos;

namespace SignalR.BusinessLayer.ValidationRules.BasketValidations
{
    public class CreateBasketValidator : AbstractValidator<CreateBasketDto>
    {
        public CreateBasketValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Ürün seçilmelidir.");

            RuleFor(x => x.RestaurantTableId)
                .GreaterThan(0).WithMessage("Masa seçilmelidir.");
        }
    }
}
