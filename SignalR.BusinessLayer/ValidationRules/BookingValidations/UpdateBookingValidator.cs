using FluentValidation;
using SignalR.DtoLayer.Dtos.BookingDtos;

namespace SignalR.BusinessLayer.ValidationRules.BookingValidations
{
    public class UpdateBookingValidator : AbstractValidator<UpdateBookingDto>
    {
        public UpdateBookingValidator()
        {
            RuleFor(x => x.BookingId)
                .GreaterThan(0).WithMessage("Geçersiz rezervasyon numarası.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez.")
                .MinimumLength(3).WithMessage("Ad soyad en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ad soyad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Mail adresi boş geçilemez.")
                .EmailAddress().WithMessage("Geçerli bir mail adresi giriniz.")
                .MaximumLength(100).WithMessage("Mail adresi en fazla 100 karakter olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş geçilemez.")
                .Matches(@"^(\+90|0)?\s?5\d{2}\s?\d{3}\s?\d{2}\s?\d{2}$")
                .WithMessage("Geçerli bir telefon numarası giriniz. (Örn: 05xx xxx xx xx)");

            RuleFor(x => x.PersonCount)
                .InclusiveBetween(1, 20)
                .WithMessage("Kişi sayısı 1 ile 20 arasında olmalıdır.");

            RuleFor(x => x.BookDate)
                .NotEmpty().WithMessage("Rezervasyon tarihi boş geçilemez.");
        }
    }
}