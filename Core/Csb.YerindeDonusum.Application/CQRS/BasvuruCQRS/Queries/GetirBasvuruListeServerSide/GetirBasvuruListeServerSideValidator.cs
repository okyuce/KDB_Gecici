using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeServerSide;

public class GetirBasvuruListeServerSideValidator : AbstractValidator<GetirBasvuruListeServerSideQuery>
{
	public GetirBasvuruListeServerSideValidator()
	{
        //RuleFor(x => x.TcKimlikNo)
        //    .Must(FluentValidationExtension.NotWhiteSpace)
        //    .WithMessage(string.Format(Messages.BOS_OLAMAZ, "TC Kimlik Numarası"));

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