using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiDenetim;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Domain.Entities;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBelge;

public class KaydetYapiDenetimCommandValidator : AbstractValidator<KaydetYapiDenetimCommand>
{
    public KaydetYapiDenetimCommandValidator()
    {
        RuleFor(x => x.BinaDegerlendirmeId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaDegerlendirmeId"));

        RuleFor(x => x.FenniMesulTc)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Fenni Mesul Tc No"))
            .When(x => x.FenniMesulSeciliMi == true);     
        RuleFor(x => x.FenniMesulTc)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Fenni Mesul Tc No"))
            .When(x => x.FenniMesulSeciliMi == true);
        
        RuleFor(x => x.YIBFNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "YIBF No"))
            .When(x => x.FenniMesulSeciliMi != true);

        RuleFor(x => x.YapiDenetimIlerlemeYuzdesi)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Yapı Denetim İlerleme Yüzdesi"));

        RuleFor(x => x.BelgeYapiDenetimDosya)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Tespit Tutanağı"));
    }
}