using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands
{
    public class UpdateIstisnaAskiKoduCommandValidator : AbstractValidator<UpdateIstisnaAskiKoduCommand>
    {
        public UpdateIstisnaAskiKoduCommandValidator()
        {
            RuleFor(x => x.Model.IstisnaAskiKoduId).GreaterThan(0);
            RuleFor(x => x.Model.AskiKodu).NotEmpty().MaximumLength(25);
            RuleFor(x => x.Model.GuncelleyenIp).NotEmpty();
            RuleFor(x => x.Model.GuncelleyenKullaniciId).GreaterThan(0);
        }
    }
}
