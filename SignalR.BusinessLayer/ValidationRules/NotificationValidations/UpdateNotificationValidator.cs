using FluentValidation;
using SignalR.DtoLayer.Dtos.NotificationDtos;

namespace SignalR.BusinessLayer.ValidationRules.NotificationValidations
{
    public class UpdateNotificationValidator : AbstractValidator<UpdateNotificationDto>
    {
        public UpdateNotificationValidator()
        {
            RuleFor(x => x.NotificationId)
                .GreaterThan(0).WithMessage("Geçerli bir kayıt bulunamadı.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Bildirim tipi boş geçilemez.")
                .MaximumLength(50).WithMessage("Bildirim tipi en fazla 50 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş geçilemez.")
                .MinimumLength(3).WithMessage("Açıklama en az 3 karakter olmalıdır.")
                .MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olabilir.");
        }
    }
}
