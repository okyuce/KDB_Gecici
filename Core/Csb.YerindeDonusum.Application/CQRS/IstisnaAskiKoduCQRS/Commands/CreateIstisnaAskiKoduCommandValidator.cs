using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands
{
    public class CreateIstisnaAskiKoduCommandValidator : AbstractValidator<CreateIstisnaAskiKoduCommand>
    {
        public CreateIstisnaAskiKoduCommandValidator()
        {
            RuleFor(x => x.Model.AskiKodu).NotEmpty().MaximumLength(25);
            RuleFor(x => x.Model.OlusturanIp).NotEmpty();
            RuleFor(x => x.Model.OlusturanKullaniciId).GreaterThan(0);
        }
    }
}
