using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruIptalTurCQRS.Queries
{
    public class GetirBasvuruIptalTurListeResponseModel
    {
        public long? BasvuruIptalTurId { get; set; }

        public string? Ad { get; set; } = null!;

        public bool? AktifMi { get; set; }

        public bool? SilindiMi { get; set; }
    }

}