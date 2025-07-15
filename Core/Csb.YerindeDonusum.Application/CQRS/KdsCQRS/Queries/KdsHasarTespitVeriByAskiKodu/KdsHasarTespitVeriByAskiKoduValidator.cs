using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByAskiKodu;

public class KdsHasarTespitVeriByAskiKoduValidator : AbstractValidator<KdsHasarTespitVeriByAskiKoduQuery>
{
	public KdsHasarTespitVeriByAskiKoduValidator()
	{
        RuleFor(x => x.AskiKodu)
			.Must(FluentValidationExtension.NotWhiteSpace)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Askı Kodu"));

	}
}