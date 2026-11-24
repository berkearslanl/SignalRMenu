using FluentValidation;
using SignalR.DtoLayer.Dtos.OrderDetailDtos;

namespace SignalR.BusinessLayer.ValidationRules.OrderDetailValidations
{
    public class CreateOrderDetailValidator : AbstractValidator<CreateOrderDetailDto>
    {
        public CreateOrderDetailValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Ürün seçilmelidir.");

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(1).WithMessage("Adet en az 1 olmalıdır.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Birim fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Toplam tutar negatif olamaz.");

            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Sipariş seçilmelidir.");
        }
    }
}
