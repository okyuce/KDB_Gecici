using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands.YapiDenetimBelgeGuncelle;

public class YapiDenetimBelgeGuncelleCommandValidator : AbstractValidator<YapiDenetimBelgeGuncelleCommand>
{
    public YapiDenetimBelgeGuncelleCommandValidator()
    {
        RuleFor(x => x.BinaYapiDenetimDosyaGuid)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaYapiDenetimDosyaGuid"));        

        RuleFor(x => x.YapiDenetimIlerlemeBelgesi)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Yapi Denetim Dosyayı boş olamaz"));
    }
}