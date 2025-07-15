using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Commands;

public class BasvuruDosyaIndirCommandValidator : AbstractValidator<BasvuruDosyaIndirCommand>
{
	public BasvuruDosyaIndirCommandValidator()
	{
		RuleFor(x => x.BasvuruDosyaId)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruDosyaId"));

		RuleFor(x => x.BasvuruDosyaId)
			.Must(FluentValidationExtension.IsStringGuid)
			.WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Dosya Bilgisi"))
			.When(x => FluentValidationExtension.NotWhiteSpace(x.BasvuruDosyaId));

		RuleFor(x => x.TcKimlikNo)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

		RuleFor(x => x.TcKimlikNo)
			.Must(FluentValidationExtension.TcDogrula)
			.WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
			.When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));
	}
}