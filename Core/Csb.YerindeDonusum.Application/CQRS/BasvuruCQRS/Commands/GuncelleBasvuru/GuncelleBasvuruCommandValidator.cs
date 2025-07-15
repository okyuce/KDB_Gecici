using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.GuncelleBasvuru;

public class GuncelleBasvuruCommandValidator : AbstractValidator<GuncelleBasvuruCommandModel>
{
    public GuncelleBasvuruCommandValidator()
    {
        RuleFor(x => x.BasvuruGuid)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Guid"));

        RuleFor(x => x.BasvuruGuid)
            .Must(FluentValidationExtension.IsStringGuid)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Başvuru Guid"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.BasvuruGuid));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));

        RuleFor(x => x.UavtIlNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT İl Kodu"));

        RuleFor(x => x.UavtIlceNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT İlçe Kodu"));

        RuleFor(x => x.UavtMahalleNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT Mahalle Kodu"));

        RuleFor(x => x.UavtCaddeNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT Cadde Kodu"));

        RuleFor(x => x.UavtDisKapiNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT Dış Kapı No"));

        RuleFor(x => x.UavtIcKapiNo)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT İc Kapı No"));

        RuleFor(x => x.UavtIlAdi)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT İl Adı"));

        RuleFor(x => x.UavtIlceAdi)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT İlçe Adı"));

        RuleFor(x => x.UavtMahalleAdi)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "UAVT Mahalle Kodu"));
    }
}