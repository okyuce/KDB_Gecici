using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBelge;

public class KaydetBelgeCommandValidator : AbstractValidator<KaydetBelgeCommand>
{
    public KaydetBelgeCommandValidator()
    {
        RuleFor(x => x.BinaDegerlendirmeId)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "BinaDegerlendirmeId"));        

        RuleFor(x => x.ImzalayanKisiSayisi)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "İmzalayan Kişi Sayısı"));
    }
}