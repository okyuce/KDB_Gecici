using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeAfadIcin;

public class GetirBasvuruListeAfadIcinValidator : AbstractValidator<GetirBasvuruListeAfadIcinQuery>
{
	public GetirBasvuruListeAfadIcinValidator()
	{
        RuleFor(x => x.BaslangicTarihi)
            .NotNull()
            .GreaterThan(DateTime.MinValue)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başlangıç Tarihi"));
        RuleFor(x => x.BitisTarihi)
            .NotNull()
            .GreaterThan(DateTime.MinValue)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Bitiş Tarihi"));

        RuleFor(x => x.Offset)
            .NotNull()
            .GreaterThanOrEqualTo(0)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Offset"));
    }
}