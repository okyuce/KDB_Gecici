using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGirisDogrula;
public class KullaniciGirisDogrulaCommandValidator : AbstractValidator<KullaniciGirisDogrulaCommand>
{
    public KullaniciGirisDogrulaCommandValidator()
    {
        RuleFor(x => x.GirisGuid)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Giriş Guid"));
        RuleFor(x => x.DogrulamaKodu)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Doğrulama Kodu"));
    }
}