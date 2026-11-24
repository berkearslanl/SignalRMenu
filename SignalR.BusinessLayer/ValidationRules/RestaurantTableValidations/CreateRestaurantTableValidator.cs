using FluentValidation;
using SignalR.DtoLayer.Dtos.RestaurantTableDtos;

namespace SignalR.BusinessLayer.ValidationRules.RestaurantTableValidations
{
    public class CreateRestaurantTableValidator : AbstractValidator<CreateRestaurantTableDto>
    {
        public CreateRestaurantTableValidator()
        {
            RuleFor(x => x.TableName)
                .NotEmpty().WithMessage("Masa adı boş geçilemez.")
                .MaximumLength(30).WithMessage("Masa adı en fazla 30 karakter olabilir.");
        }
    }
}
