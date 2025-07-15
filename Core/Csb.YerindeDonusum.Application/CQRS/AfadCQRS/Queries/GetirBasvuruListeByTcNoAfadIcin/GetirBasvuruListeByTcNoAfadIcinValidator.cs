using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeByTcNoAfadIcin;

public class GetirBasvuruListeByTcNoAfadIcinValidator : AbstractValidator<GetirBasvuruListeByTcNoAfadIcinQuery>
{
	public GetirBasvuruListeByTcNoAfadIcinValidator()
	{
        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));
    }
}