using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;

public class GetirKisiBilgileriTcDenQueryValidator : AbstractValidator<GetirKisiBilgileriTcDenQuery>
{
    public GetirKisiBilgileriTcDenQueryValidator()
    {
        RuleFor(x => x.TcKimlikNo)
           .Must(FluentValidationExtension.NotEmpty)
           .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotEmpty(x.TcKimlikNo));

        RuleFor(x => x.DogumTarih)
            .NotNull()
            .GreaterThan(DateTime.MinValue)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Doğum Tarihi"));

        RuleFor(x => x.DogumTarih)
            .NotNull()
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Doğum Tarihi"));
    }
}