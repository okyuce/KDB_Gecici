using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadTopluBasvuruListe;

public class GetirAfadTopluBasvuruListeQueryValidator : AbstractValidator<GetirAfadTopluBasvuruListeQuery>
{
	public GetirAfadTopluBasvuruListeQueryValidator()
	{
        RuleFor(x => x.Tarih)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Tarih"));
    }
}