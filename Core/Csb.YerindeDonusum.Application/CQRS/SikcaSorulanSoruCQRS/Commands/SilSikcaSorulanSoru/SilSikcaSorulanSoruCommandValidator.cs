using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands.SilSikcaSorulanSoru;

public class SilSikcaSorulanSoruCommandValidator : AbstractValidator<SilSikcaSorulanSoruCommand>
{
	public SilSikcaSorulanSoruCommandValidator()
	{
		RuleFor(x => x.SikcaSorulanSoruId)
			.Must(FluentValidationExtension.NotEmpty)
			.WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "SikcaSorulanSoruId"));
	}
}