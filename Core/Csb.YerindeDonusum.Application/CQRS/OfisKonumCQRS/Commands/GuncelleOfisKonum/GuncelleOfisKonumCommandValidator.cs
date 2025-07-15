using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands.GuncelleOfisKonum;

public class GuncelleOfisKonumCommandValidator : AbstractValidator<GuncelleOfisKonumCommand>
{
	public GuncelleOfisKonumCommandValidator()
	{
		RuleFor(x => x.OfisKonumId)
			.Must(FluentValidationExtension.NotEmpty)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "OfisKonumId"));
        
        RuleFor(x => x.IlAdi)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "İl"));

        RuleFor(x => x.IlceAdi)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "İlçe"));

        RuleFor(x => x.Adres)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Adres"));

        RuleFor(x => x.Konum)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Konum"))
            .MinimumLength(12)
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "Konum"));

        RuleFor(x => x.AktifMi)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Durum"));
    }
}