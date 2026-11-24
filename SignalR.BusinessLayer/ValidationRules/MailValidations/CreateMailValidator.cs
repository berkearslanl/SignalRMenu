using FluentValidation;
using SignalR.DtoLayer.Dtos.MailDtos;

namespace SignalR.BusinessLayer.ValidationRules.MailValidations
{
    public class CreateMailValidator : AbstractValidator<CreateMailDto>
    {
        public CreateMailValidator()
        {
            RuleFor(x => x.ReceiveMail)
                .NotEmpty().WithMessage("Alıcı mail adresi boş geçilemez.")
                .MaximumLength(100).WithMessage("Alıcı mail adresi en fazla 100 karakter olabilir.")
                .EmailAddress().WithMessage("Geçerli bir mail adresi giriniz.");

            RuleFor(x => x.ReceiveName)
                .NotEmpty().WithMessage("Alıcı adı boş geçilemez.")
                .MinimumLength(2).WithMessage("Alıcı adı en az 2 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Alıcı adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Konu boş geçilemez.")
                .MinimumLength(3).WithMessage("Konu en az 3 karakter olmalıdır.")
                .MaximumLength(150).WithMessage("Konu en fazla 150 karakter olabilir.");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Mesaj içeriği boş geçilemez.")
                .MinimumLength(5).WithMessage("Mesaj içeriği en az 5 karakter olmalıdır.")
                .MaximumLength(4000).WithMessage("Mesaj içeriği en fazla 4000 karakter olabilir.");
        }
    }
}
