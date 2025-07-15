using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AdaParselGuncelle;

public class BasvuruAdaParselGuncelleCommandValidator : AbstractValidator<BasvuruAdaParselGuncelleCommandModel>
{
    public BasvuruAdaParselGuncelleCommandValidator()
    {
        RuleFor(x => x.TapuBeyanIlceId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "İlçe Boş Olamaz"));
        
        RuleFor(x => x.TapuBeyanMahalleId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Mahalle Boş Olamaz"));

        RuleFor(x => x.TapuBeyanAda)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Ada Boş Olamaz"));

        RuleFor(x => x.TapuBeyanParsel)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Parsel Boş Olamaz"));
    }
}