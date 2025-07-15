using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeEkle;

public class BinaOdemeEkleCommandValidator : AbstractValidator<BinaOdemeEkleCommand>
{
    public BinaOdemeEkleCommandValidator()
    {
        RuleFor(x => x.BinaDegerlendirmeId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Bina Degerlendirme Id"));

        RuleFor(x => x.Seviye)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Seviye"))
            .When(x=> x.YapimaYonelikDigerHibeOdemesiMi != true);
    }
}