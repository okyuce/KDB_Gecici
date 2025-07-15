using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AktarKamuUstlenecek;

public class AktarKamuUstlenecekCommandValidator : AbstractValidator<AktarKamuUstlenecekCommand>
{
    public AktarKamuUstlenecekCommandValidator()
    {
        RuleFor(x => x.BasvuruId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruId"));
    }
}