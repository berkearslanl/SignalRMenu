using FluentValidation;
using SignalR.DtoLayer.Dtos.MessageDtos;

namespace SignalR.BusinessLayer.ValidationRules.MessageValidations
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageDto>
    {
        public CreateMessageValidator()
        {
            RuleFor(x => x.NameSurname)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez.")
                .MinimumLength(3).WithMessage("Ad soyad en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ad soyad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş geçilemez.")
                .Matches(@"^(\+90|0)?\s?5\d{2}\s?\d{3}\s?\d{2}\s?\d{2}$")
                .WithMessage("Geçerli bir telefon numarası giriniz. (Örn: 05xx xxx xx xx)");

            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage("Mail adresi boş geçilemez.")
                .MaximumLength(100).WithMessage("Mail adresi en fazla 100 karakter olabilir.")
                .EmailAddress().WithMessage("Geçerli bir mail adresi giriniz.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Konu boş geçilemez.")
                .MinimumLength(3).WithMessage("Konu en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Konu en fazla 100 karakter olabilir.");

            RuleFor(x => x.MessageContent)
                .NotEmpty().WithMessage("Mesaj içeriği boş geçilemez.")
                .MinimumLength(5).WithMessage("Mesaj içeriği en az 5 karakter olmalıdır.")
                .MaximumLength(1000).WithMessage("Mesaj içeriği en fazla 1000 karakter olabilir.");
        }
    }
}
