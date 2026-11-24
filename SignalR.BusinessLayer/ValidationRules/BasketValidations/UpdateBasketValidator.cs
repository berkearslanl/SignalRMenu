using FluentValidation;
using SignalR.DtoLayer.Dtos.BasketDtos;

namespace SignalR.BusinessLayer.ValidationRules.BasketValidations
{
    public class UpdateBasketValidator : AbstractValidator<UpdateBasketDto>
    {
        public UpdateBasketValidator()
        {
            RuleFor(x => x.BasketId)
                .GreaterThan(0).WithMessage("Geçerli bir kayıt bulunamadı.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Ürün seçilmelidir.");

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(1).WithMessage("Adet en az 1 olmalıdır.");

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Toplam tutar negatif olamaz.");

            RuleFor(x => x.RestaurantTableId)
                .GreaterThan(0).WithMessage("Masa seçilmelidir.");
        }
    }
}
