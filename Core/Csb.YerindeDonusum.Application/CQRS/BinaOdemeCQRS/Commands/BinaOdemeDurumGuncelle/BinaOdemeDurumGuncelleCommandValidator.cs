using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeDurumGuncelle;

public class BinaOdemeDurumGuncelleCommandValidator : AbstractValidator<BinaOdemeDurumGuncelleCommand>
{
    public BinaOdemeDurumGuncelleCommandValidator()
    {    
        RuleFor(x => x.BinaOdemeIds)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaOdemeIds"));
        
        RuleFor(x => x.BinaOdemeDurumId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaOdemeDurumId"));
        
        RuleFor(x => x.ReddetmeSebebi)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Reddetme Sebebi"))
            .When(x=> x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Reddedildi);
    }
}