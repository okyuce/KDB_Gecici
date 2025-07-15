using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByUid;

public class KdsHasarTespitVeriByUidValidator : AbstractValidator<KdsHasarTespitVeriByUidQuery>
{
	public KdsHasarTespitVeriByUidValidator()
	{
        RuleFor(x => x.HasarTespitUid)
             .Must(FluentValidationExtension.NotWhiteSpace)
             .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Hasar Tespit Guid"));

        RuleFor(x => x.HasarTespitUid)
            .Must(FluentValidationExtension.IsStringGuid)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Hasar Tespit Guid"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.HasarTespitUid));

    }
}