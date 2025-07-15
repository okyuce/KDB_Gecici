using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirAfadBasvuruById;

public class GetirAfadBasvuruByIdQueryValidator : AbstractValidator<GetirAfadBasvuruByIdQuery>
{
	public GetirAfadBasvuruByIdQueryValidator()
	{
        RuleFor(x => x.CsbId)
             .Must(FluentValidationExtension.NotEmpty)
             .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Afad Başvuru Id"));
    }
}