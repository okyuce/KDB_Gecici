using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands
{
    public class DeleteIstisnaAskiKoduCommandValidator : AbstractValidator<DeleteIstisnaAskiKoduCommand>
    {
        public DeleteIstisnaAskiKoduCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
