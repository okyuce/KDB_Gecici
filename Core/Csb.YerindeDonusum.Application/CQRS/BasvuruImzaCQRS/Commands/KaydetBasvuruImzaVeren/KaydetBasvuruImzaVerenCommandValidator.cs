using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVeren;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands;

public class KaydetBasvuruImzaVerenCommandValidator : AbstractValidator<KaydetBasvuruImzaVerenCommand>
{
	public KaydetBasvuruImzaVerenCommandValidator()
	{
        RuleFor(x => x.BasvuruId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruId"))
            .When(x => !FluentValidationExtension.NotEmpty(x.BasvuruKamuUstlenecekId));

        RuleFor(x => x.BasvuruKamuUstlenecekId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruKamuUstlenecekId"))
            .When(x => !FluentValidationExtension.NotEmpty(x.BasvuruId));

        RuleFor(x => x.BagimsizBolumAlani)
			.Must(FluentValidationExtension.NotEmpty)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Bağımsız Bölüm Alanı"));		
		
		RuleFor(x => x.BagimsizBolumNo)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Bağımsız Bölüm No"));

        RuleFor(x => x.SozlesmeTarihi)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Kredi Sözleşme Tarihi"))
            .When(x => (x.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi || x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi) && !x.KendimKarsilamakIstiyorum);
    }
}