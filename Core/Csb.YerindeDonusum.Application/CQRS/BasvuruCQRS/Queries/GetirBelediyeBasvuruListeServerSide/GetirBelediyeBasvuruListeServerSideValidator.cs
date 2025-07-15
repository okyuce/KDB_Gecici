using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBelediyeBasvuruListeServerSide;

public class GetirBelediyeBasvuruListeServerSideValidator : AbstractValidator<GetirBelediyeBasvuruListeServerSideQuery>
{
	public GetirBelediyeBasvuruListeServerSideValidator()
	{
        RuleFor(x => x.HasarTespitAskiKodu)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage("Askı Kodu boş olamaz!");

        //RuleFor(x => x.TcKimlikNo)
        //    .Must(FluentValidationExtension.TcDogrula)
        //    .WithMessage(string.Format(Messages.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
        //    .When(x => FluentValidationExtension.NotWhiteSpace(x.TcKimlikNo));  

        //RuleFor(x => x.TuzelKisiMersisNo)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(Messages.HATALI_VEYA_GECERSIZ, "Mersis No"))
        //    .When(x => x.TuzelMi == true);
    }
}