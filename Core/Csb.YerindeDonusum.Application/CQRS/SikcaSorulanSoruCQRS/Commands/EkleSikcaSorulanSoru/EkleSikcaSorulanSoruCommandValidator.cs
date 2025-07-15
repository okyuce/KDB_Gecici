using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands.EkleSikcaSorulanSoru;

public class EkleSikcaSorulanSoruCommandValidator : AbstractValidator<EkleSikcaSorulanSoruCommand>
{
	public EkleSikcaSorulanSoruCommandValidator()
	{
		RuleFor(x => x.Soru)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Soru"))
			.MaximumLength(500).WithMessage(string.Format(ValidationMessageConstants.MAKSIMUM_KARAKTER_SAYISI, "Soru","500"));
		
		RuleFor(x => x.Cevap)
			.Must(x=> FluentValidationExtension.NotWhiteSpace(StringAddons.StripHTML(x)))
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Cevap"));
	}
}