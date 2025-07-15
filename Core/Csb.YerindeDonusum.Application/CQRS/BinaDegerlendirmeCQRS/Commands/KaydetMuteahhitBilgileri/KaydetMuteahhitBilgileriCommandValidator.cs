using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetMuteahhitBilgileri;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBelge;

public class KaydetMuteahhitBilgileriCommandValidator : AbstractValidator<KaydetMuteahhitBilgileriCommand>
{
    public KaydetMuteahhitBilgileriCommandValidator()
    {
        RuleFor(x => x.BinaDegerlendirmeId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaDegerlendirmeId"));

        RuleFor(x => x.IbanNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "IBAN No"));

        RuleFor(x => x.IbanNo)
            .Must(FluentValidationExtension.IbanDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "IBAN No"));

        RuleFor(x => x.BinaMuteahhitTapuTurId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaMuteahhitTapuTurId"));
    }
}