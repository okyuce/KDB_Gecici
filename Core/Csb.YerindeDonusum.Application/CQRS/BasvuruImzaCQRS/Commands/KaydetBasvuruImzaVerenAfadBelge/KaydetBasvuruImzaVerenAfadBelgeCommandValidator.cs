using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenAfadBelge;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenAfadBelge;

public class KaydetBasvuruImzaVerenAfadBelgeCommandValidator : AbstractValidator<KaydetBasvuruImzaVerenAfadBelgeCommand>
{
	public KaydetBasvuruImzaVerenAfadBelgeCommandValidator()
	{
        RuleFor(x => x.BasvuruId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Id"));

        RuleFor(x => x.AfadBelge)
            .Must(x => x?.DosyaUzanti?.Substring(0, 1) == ".")
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "AFAD Belgesi"))
            .Must(x => FluentValidationExtension.NotEmpty(x?.DosyaBase64))
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "AFAD Belgesi"));
    }
}