using FluentValidation;
using SignalR.DtoLayer.Dtos.OrderDtos;

namespace SignalR.BusinessLayer.ValidationRules.OrderValidations
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.RestaurantTableId)
                .GreaterThan(0).WithMessage("Masa seçilmelidir.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olabilir.");

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Toplam tutar negatif olamaz.");
        }
    }
}
