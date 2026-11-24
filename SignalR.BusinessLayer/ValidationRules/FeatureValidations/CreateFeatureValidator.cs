using FluentValidation;
using SignalR.DtoLayer.Dtos.FeatureDtos;

namespace SignalR.BusinessLayer.ValidationRules.FeatureValidations
{
    public class CreateFeatureValidator : AbstractValidator<CreateFeatureDto>
    {
        public CreateFeatureValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş geçilemez.")
                .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş geçilemez.")
                .MinimumLength(5).WithMessage("Açıklama en az 5 karakter olmalıdır.")
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Görsel adresi boş geçilemez.")
                .MaximumLength(500).WithMessage("Görsel adresi en fazla 500 karakter olabilir.");
        }
    }
}
