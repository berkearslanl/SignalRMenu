using FluentValidation;
using SignalR.DtoLayer.Dtos.ContactDtos;

namespace SignalR.BusinessLayer.ValidationRules.ContactValidations
{
    public class CreateContactValidator : AbstractValidator<CreateContactDto>
    {
        public CreateContactValidator()
        {
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Adres boş geçilemez.")
                .MinimumLength(5).WithMessage("Adres en az 5 karakter olmalıdır.")
                .MaximumLength(200).WithMessage("Adres en fazla 200 karakter olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş geçilemez.")
                .Matches(@"^[\d\s\+\-\(\)]{7,20}$")
                .WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage("Mail adresi boş geçilemez.")
                .MaximumLength(100).WithMessage("Mail adresi en fazla 100 karakter olabilir.")
                .EmailAddress().WithMessage("Geçerli bir mail adresi giriniz.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş geçilemez.")
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
