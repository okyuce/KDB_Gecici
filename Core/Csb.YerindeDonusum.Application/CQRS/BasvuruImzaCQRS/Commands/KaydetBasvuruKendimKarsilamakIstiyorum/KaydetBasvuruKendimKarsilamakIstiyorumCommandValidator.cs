using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruKendimKarsilamakIstiyorum;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands;

public class KaydetBasvuruKendimKarsilamakIstiyorumCommandValidator : AbstractValidator<KaydetBasvuruKendimKarsilamakIstiyorumCommand>
{
	public KaydetBasvuruKendimKarsilamakIstiyorumCommandValidator()
	{
        RuleFor(x => x.BasvuruId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruId"))
            .When(x => !FluentValidationExtension.NotEmpty(x.BasvuruKamuUstlenecekId));

        RuleFor(x => x.BasvuruKamuUstlenecekId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruKamuUstlenecekId"))
            .When(x => !FluentValidationExtension.NotEmpty(x.BasvuruId));

    }
}