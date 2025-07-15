using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.KaydetTebligatDonderimDetayDosya;

public class KaydetTebligatGonderimDetayDosyaCommandValidator : AbstractValidator<KaydetTebligatGonderimDetayDosyaCommand>
{
    public KaydetTebligatGonderimDetayDosyaCommandValidator()
    {
        RuleFor(x => x.TebligatGonderimDetayId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TebligatGonderimDetayId"));

        RuleFor(x => x.TebligatSozlesmesi)
            .Must(x => x?.DosyaUzanti?.Substring(0, 1) == ".")
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Tebligat Gönderim Detay Dosya"))
            .When(x => FluentValidationExtension.NotEmpty(x.TebligatSozlesmesi));
    }
}
