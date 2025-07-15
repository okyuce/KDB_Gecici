using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciTokenYenile;

public class KullaniciTokenYenileCommandValidator : AbstractValidator<KullaniciTokenYenileCommand>
{
	public KullaniciTokenYenileCommandValidator()
	{
		RuleFor(x => x.AccessToken)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Access Token"));

		RuleFor(x => x.RefreshToken)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Refresh Token"));
	}
}