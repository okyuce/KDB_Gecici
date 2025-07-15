using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapiDisKapiNo;

public class GuncelleYeniYapiDisKapiNoCommandValidator : AbstractValidator<GuncelleYeniYapiDisKapiNoCommand>
{
    public GuncelleYeniYapiDisKapiNoCommandValidator()
    {
        //RuleFor(x => x.UavtMahalleNo)
        //    .Must(FluentValidationExtension.NotEmpty)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Uavt Mahalle No"));

        //RuleFor(x => x.HasarTespitAskiKodu)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "ASKI Kodu"));

        //RuleFor(x => x.BinaDisKapiNo)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Dış Kapı No"));
    }
}