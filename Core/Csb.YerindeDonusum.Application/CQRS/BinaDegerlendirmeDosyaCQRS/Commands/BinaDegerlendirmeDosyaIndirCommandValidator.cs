using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands;

public class BinaDegerlendirmeDosyaIndirCommandValidator : AbstractValidator<BinaDegerlendirmeDosyaIndirCommand>
{
	public BinaDegerlendirmeDosyaIndirCommandValidator()
	{
		RuleFor(x => x.BinaDegerlendirmeDosyaId)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaDegerlendirmeDosyaId"));

		RuleFor(x => x.BinaDegerlendirmeDosyaId)
			.Must(FluentValidationExtension.IsStringGuid)
			.WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Dosya Bilgisi"))
			.When(x => FluentValidationExtension.NotWhiteSpace(x.BinaDegerlendirmeDosyaId));

	}
}