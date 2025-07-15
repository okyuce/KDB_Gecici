using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruSonuclandir;

public class BasvuruSonuclandirCommandValidator : AbstractValidator<BasvuruSonuclandirCommand>
{
    public BasvuruSonuclandirCommandValidator()
    {
        //RuleFor(x => x.BasvuruDurumId)
        //    .Must(FluentValidationExtension.NotEmpty)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Durumu"));

        RuleFor(x => x.BasvuruId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruId"))
            .When(x => !FluentValidationExtension.NotEmpty(x.BasvuruKamuUstlenecekId));

        RuleFor(x => x.BasvuruKamuUstlenecekId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BasvuruKamuUstlenecekId"))
            .When(x => !FluentValidationExtension.NotEmpty(x.BasvuruId));

        //RuleFor(x => x.SonuclandirmaAciklamasi)
        //    .MinimumLength(25)
        //    .WithMessage(string.Format(ValidationMessageConstants.MINIMUM_KARAKTER_SAYISI, "Açıklama", 25));
        //RuleFor(x => x.SonuclandirmaAciklamasi)
        //    .Must(FluentValidationExtension.NotOnlySpecialChars)
        //    .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Açıklama"))
        //    .When(x => FluentValidationExtension.NotWhiteSpace(x.SonuclandirmaAciklamasi));

        //RuleFor(x => x.TcKimlikNo)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));
        //RuleFor(x => x.TcKimlikNo)
        //    .Must(FluentValidationExtension.TcDogrula)
        //    .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
        //    .When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));

        //RuleFor(x => x.BasvuruGuid)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Başvuru Guid"));
        //RuleFor(x => x.BasvuruGuid)
        //    .Must(FluentValidationExtension.IsStringGuid)
        //    .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Başvuru Guid"))
        //    .When(x => FluentValidationExtension.NotWhiteSpace(x.BasvuruGuid));
    }
}