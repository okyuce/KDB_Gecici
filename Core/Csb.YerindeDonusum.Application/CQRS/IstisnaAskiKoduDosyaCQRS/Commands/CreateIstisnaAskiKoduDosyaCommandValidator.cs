using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class CreateIstisnaAskiKoduDosyaCommandValidator : AbstractValidator<CreateIstisnaAskiKoduDosyaCommand>
    {
        public CreateIstisnaAskiKoduDosyaCommandValidator()
        {
            RuleFor(x => x.Model.IstisnaAskiKoduId).GreaterThan(0);
            RuleFor(x => x.Model.DosyaAdi).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Model.DosyaYolu).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Model.DosyaTuru).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Model.OlusturanIp).NotEmpty();
            RuleFor(x => x.Model.OlusturanKullaniciId).GreaterThan(0);
        }
    }
}
