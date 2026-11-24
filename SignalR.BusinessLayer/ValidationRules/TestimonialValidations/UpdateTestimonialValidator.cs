using FluentValidation;
using SignalR.DtoLayer.Dtos.TestimonialDtos;

namespace SignalR.BusinessLayer.ValidationRules.TestimonialValidations
{
    public class UpdateTestimonialValidator : AbstractValidator<UpdateTestimonialDto>
    {
        public UpdateTestimonialValidator()
        {
            RuleFor(x => x.TestimonialId)
                .GreaterThan(0).WithMessage("Geçerli bir kayıt bulunamadı.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez.")
                .MinimumLength(3).WithMessage("Ad soyad en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ad soyad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Ünvan boş geçilemez.")
                .MinimumLength(2).WithMessage("Ünvan en az 2 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Ünvan en fazla 100 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum boş geçilemez.")
                .MinimumLength(5).WithMessage("Yorum en az 5 karakter olmalıdır.")
                .MaximumLength(500).WithMessage("Yorum en fazla 500 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Görsel adresi boş geçilemez.")
                .MaximumLength(500).WithMessage("Görsel adresi en fazla 500 karakter olabilir.");
        }
    }
}
