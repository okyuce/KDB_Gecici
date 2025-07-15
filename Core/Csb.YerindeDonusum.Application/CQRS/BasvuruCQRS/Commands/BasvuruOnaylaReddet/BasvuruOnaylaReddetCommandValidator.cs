using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruOnaylaReddet;

public class BasvuruOnaylaReddetCommandValidator : AbstractValidator<BasvuruOnaylaReddetCommand>
{
    public BasvuruOnaylaReddetCommandValidator()
    {
        RuleFor(x => x.BasvuruIds)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Listesi"));

        RuleFor(x => x.Onayla)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Onay Durumu"));
    }
}