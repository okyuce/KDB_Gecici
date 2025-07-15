using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeByTcNo;

public class GetirBasvuruListeByTcNoValidator : AbstractValidator<GetirBasvuruListeByTcNoQueryModel>
{
	public GetirBasvuruListeByTcNoValidator()
	{
        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));
        
        RuleFor(x => x.TuzelKisiMersisNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Mersis No"))
            .When(x => x.TuzelMi == true);
    }
}