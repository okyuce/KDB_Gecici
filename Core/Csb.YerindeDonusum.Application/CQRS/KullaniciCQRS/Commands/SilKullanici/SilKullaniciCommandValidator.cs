using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands.SilKullanici;

public class SilKullaniciCommandValidator : AbstractValidator<SilKullaniciCommand>
{

	public SilKullaniciCommandValidator()
	{
		RuleFor(x => x.KullaniciId)
			.Must(FluentValidationExtension.NotEmpty)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "KullaniciId"));

	}
}