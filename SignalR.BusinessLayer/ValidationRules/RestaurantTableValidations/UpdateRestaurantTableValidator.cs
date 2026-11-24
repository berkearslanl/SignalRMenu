using FluentValidation;
using SignalR.DtoLayer.Dtos.RestaurantTableDtos;

namespace SignalR.BusinessLayer.ValidationRules.RestaurantTableValidations
{
    public class UpdateRestaurantTableValidator : AbstractValidator<UpdateRestaurantTableDto>
    {
        public UpdateRestaurantTableValidator()
        {
            RuleFor(x => x.RestaurantTableId)
                .GreaterThan(0).WithMessage("Geçerli bir kayıt bulunamadı.");

            RuleFor(x => x.TableName)
                .NotEmpty().WithMessage("Masa adı boş geçilemez.")
                .MaximumLength(30).WithMessage("Masa adı en fazla 30 karakter olabilir.");
        }
    }
}
