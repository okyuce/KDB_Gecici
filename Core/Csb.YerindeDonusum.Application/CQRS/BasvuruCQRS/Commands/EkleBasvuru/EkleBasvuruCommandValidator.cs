using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;

public class EkleBasvuruCommandValidator : AbstractValidator<EkleBasvuruCommandModel>
{
    public EkleBasvuruCommandValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "İstek"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(FluentValidationExtension.TcDogrula)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));

        RuleFor(x => x)
            .Must(x => FluentValidationExtension.NotWhiteSpace(x.TapuIlAdi)
                   && FluentValidationExtension.NotWhiteSpace(x.TapuIlceAdi)
                   && FluentValidationExtension.NotWhiteSpace(x.TapuMahalleAdi))
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Tapu İl/İlçe/Mahalle"));

        RuleFor(x => x.VatandaslikDurumu)
            .Must(x => FluentValidationExtension.InclusiveBetween(x, 1, 3))
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Vatandaşlık Durumu"));

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

        RuleFor(x => x.CepTelefonu)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Cep Telefonu"));

        RuleFor(x => x.CepTelefonu)
            .Must(FluentValidationExtension.ValidatePhone)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Cep Telefonu"));

        RuleFor(x => x.BasvuruDestekTurId)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Destek Türü"));

        RuleFor(x => x.BasvuruDestekTurId)
            .Must(FluentValidationExtension.IsStringGuid)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Başvuru Destek Türü"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.BasvuruDestekTurId));

        RuleFor(x => x.BasvuruTurId)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Türü"));

        RuleFor(x => x.BasvuruTurId)
            .Must(FluentValidationExtension.IsStringGuid)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Başvuru Türü"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.BasvuruTurId));

        RuleFor(x => x.BasvuruKanalId)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Kanalı"));

        RuleFor(x => x.BasvuruKanalId)
            .Must(FluentValidationExtension.IsStringGuid)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Başvuru Kanalı"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.BasvuruKanalId));

        RuleFor(x => x.AydinlatmaMetniId)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Aydinlatma Metni"));

        RuleFor(x => x.AydinlatmaMetniId)
            .Must(FluentValidationExtension.IsStringGuid)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Aydinlatma Metni"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.AydinlatmaMetniId));

        RuleFor(x => x.TuzelKisiTipId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Tüzel Kişi Tipi"))
            .When(x => FluentValidationExtension.NotWhiteSpace(x.TuzelKisiMersisNo));

        RuleFor(x => x.TuzelKisiMersisNo)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Tüzel Kişiye Ait Mersis Numarası"))
            .When(x => FluentValidationExtension.NotEmpty(x.TuzelKisiTipId) && x.BasvuruKanalId == Enums.BasvuruKanalEnum.EDevlet.ToString());

        RuleForEach(x => x.BasvuruDosyaListe)
            .Must(x => FluentValidationExtension.NotEmpty(x.DosyaUzanti))
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Dosya Uzantı"))
            .When(x => x.BasvuruDosyaListe?.Any() == true);

        RuleForEach(x => x.BasvuruTuzelYetkiDosyaListe)
            .Must(x => FluentValidationExtension.NotEmpty(x.DosyaUzanti))
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Dosya Uzantı"))
            .When(x => x.BasvuruTuzelYetkiDosyaListe?.Any() == true);

        RuleForEach(x => x.BasvuruDosyaListe)
            .Must(x => x?.DosyaUzanti?.Substring(0, 1) == ".")
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Dosya Uzantı"))
            .When(x => x.BasvuruDosyaListe?.Any() == true);
        
        RuleForEach(x => x.BasvuruTuzelYetkiDosyaListe)
            .Must(x => x?.DosyaUzanti?.Substring(0, 1) == ".")
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Dosya Uzantı"))
            .When(x => x.BasvuruTuzelYetkiDosyaListe?.Any() == true);

        RuleForEach(x => x.BasvuruDosyaListe)
            .Must(x => FluentValidationExtension.NotEmpty(x.DosyaBase64))
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Dosya İçeriği"))
            .When(x => x.BasvuruDosyaListe?.Any() == true);
        
        RuleForEach(x => x.BasvuruTuzelYetkiDosyaListe)
            .Must(x => FluentValidationExtension.NotEmpty(x.DosyaBase64))
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Dosya İçeriği"))
            .When(x => x.BasvuruTuzelYetkiDosyaListe?.Any() == true);
    }
}