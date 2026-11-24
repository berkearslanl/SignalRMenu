using FluentValidation;
using SignalR.DtoLayer.Dtos.AboutDtos;

namespace SignalR.BusinessLayer.ValidationRules.AboutValidations
{
    public class UpdateAboutValidator : AbstractValidator<UpdateAboutDto>
    {
        public UpdateAboutValidator()
        {
            RuleFor(x => x.AboutId)
                .GreaterThan(0).WithMessage("Geçerli bir kayıt bulunamadı.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş geçilemez.")
                .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş geçilemez.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır.")
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Görsel adresi boş geçilemez.")
                .MaximumLength(500).WithMessage("Görsel adresi en fazla 500 karakter olabilir.");
        }
    }
}
