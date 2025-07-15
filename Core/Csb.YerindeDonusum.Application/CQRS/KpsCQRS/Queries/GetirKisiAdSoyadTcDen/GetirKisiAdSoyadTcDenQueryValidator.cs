using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;

public class GetirKisiAdSoyadTcDenQueryValidator : AbstractValidator<GetirKisiAdSoyadTcDenQuery>
{
    public GetirKisiAdSoyadTcDenQueryValidator()
    {
        RuleFor(x => x.TcKimlikNo)
           .Must(FluentValidationExtension.NotEmpty)
           .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotEmpty(x.TcKimlikNo));
    }
}