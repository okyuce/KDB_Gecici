using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenSozlesme;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands;

public class KaydetBasvuruImzaVerenSozlesmeCommandValidator : AbstractValidator<KaydetBasvuruImzaVerenSozlesmeCommand>
{
	public KaydetBasvuruImzaVerenSozlesmeCommandValidator()
	{
        RuleFor(x => x.BasvuruImzaVerenId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruImzaVerenId"));

        RuleFor(x => x.KrediSozlesmesi)
            .Must(x => x?.DosyaUzanti?.Substring(0, 1) == ".")
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Kredi Sözleşmesi"))
            .When(x => FluentValidationExtension.NotEmpty(x.KrediSozlesmesi));

        RuleFor(x => x.HibeSozlesmesi)
            .Must(x => x?.DosyaUzanti?.Substring(0, 1) == ".")
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Hibe Onayı"))
            .When(x => FluentValidationExtension.NotEmpty(x.HibeSozlesmesi));

        RuleFor(x => x.HibeSozlesmesi)
            .Must(x => x?.DosyaUzanti?.Substring(0, 1) == ".")
            .WithMessage(string.Format(ValidationMessageConstants.EN_AZ_BIRI_BOS_OLAMAZ, "Hibe Onayı", "Kredi Sözleşmesi"))
            .When(x => FluentValidationExtension.IsEmpty(x.KrediSozlesmesi) && FluentValidationExtension.IsEmpty(x.HibeSozlesmesi));
    }
}