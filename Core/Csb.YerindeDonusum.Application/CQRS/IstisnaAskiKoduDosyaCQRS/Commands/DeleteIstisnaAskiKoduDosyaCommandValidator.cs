using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class DeleteIstisnaAskiKoduDosyaCommandValidator : AbstractValidator<DeleteIstisnaAskiKoduDosyaCommand> { public DeleteIstisnaAskiKoduDosyaCommandValidator() => RuleFor(x => x.Id).GreaterThan(0); }
}
