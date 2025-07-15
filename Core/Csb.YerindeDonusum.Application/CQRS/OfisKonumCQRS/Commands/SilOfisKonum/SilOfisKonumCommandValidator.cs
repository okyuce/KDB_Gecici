using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands.SilOfisKonum;

public class SilOfisKonumCommandValidator : AbstractValidator<SilOfisKonumCommand>
{
	public SilOfisKonumCommandValidator()
	{
		RuleFor(x => x.OfisKonumId)
			.Must(FluentValidationExtension.NotEmpty)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "OfisKonumId"));
	}
}