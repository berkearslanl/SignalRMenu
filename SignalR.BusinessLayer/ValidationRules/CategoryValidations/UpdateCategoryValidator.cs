using FluentValidation;
using SignalR.DtoLayer.Dtos.CategoryDtos;

namespace SignalR.BusinessLayer.ValidationRules.CategoryValidations
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kayıt bulunamadı.");

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori adı boş geçilemez.")
                .MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kategori adı en fazla 50 karakter olabilir.");
        }
    }
}
