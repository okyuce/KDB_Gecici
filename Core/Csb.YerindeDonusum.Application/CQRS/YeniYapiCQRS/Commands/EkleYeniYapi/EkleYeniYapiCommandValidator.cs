using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.EkleYeniYapi;

public class EkleYeniYapiCommandValidator : AbstractValidator<EkleYeniYapiCommand>
{
    public EkleYeniYapiCommandValidator()
    {
        RuleFor(x => x.UavtMahalleNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Uavt Mahalle No"));

        //RuleFor(x => x.Ada)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Ada"));

        //RuleFor(x => x.Parsel)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Parsel"));

        RuleFor(x => x.BinaDisKapiNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Dış Kapı No"));
    }
}