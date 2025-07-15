using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.SilYeniYapi;

public class SilYeniYapiCommandValidator : AbstractValidator<SilYeniYapiCommand>
{

	public SilYeniYapiCommandValidator()
	{
		RuleFor(x => x.BinaDegerlendirmeId)
			.Must(FluentValidationExtension.NotEmpty)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaDegerlendirmeId"));

	}
}