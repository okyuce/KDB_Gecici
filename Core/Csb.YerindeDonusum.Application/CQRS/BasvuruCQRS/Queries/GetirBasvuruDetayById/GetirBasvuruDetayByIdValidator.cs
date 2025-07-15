using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;

public class GetirBasvuruDetayByIdValidator : AbstractValidator<GetirBasvuruDetayByIdQueryModel>
{
	public GetirBasvuruDetayByIdValidator()
	{
        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));

        RuleFor(x => x.BasvuruId)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Guid"));

        RuleFor(x => x.BasvuruId)
            .Must(FluentValidationExtension.IsStringGuid)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Başvuru Guid"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.BasvuruId));
    }
}