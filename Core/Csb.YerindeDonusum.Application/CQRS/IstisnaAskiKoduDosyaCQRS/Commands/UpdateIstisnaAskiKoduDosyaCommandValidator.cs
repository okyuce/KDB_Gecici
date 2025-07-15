using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class UpdateIstisnaAskiKoduDosyaCommandValidator : AbstractValidator<UpdateIstisnaAskiKoduDosyaCommand>
    {
        public UpdateIstisnaAskiKoduDosyaCommandValidator()
        {
            RuleFor(x => x.Model.IstisnaAskiKoduDosyaId).GreaterThan(0);
            RuleFor(x => x.Model.DosyaAdi).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Model.DosyaYolu).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Model.DosyaTuru).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Model.GuncelleyenIp).NotEmpty();
            RuleFor(x => x.Model.GuncelleyenKullaniciId).GreaterThan(0);
        }
    }
}
