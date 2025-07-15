using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.CQRS.DestekTurCQRS.Queries
{
    public class GetirBasvuruDestekTurListeResponseModel
    {
        public long? BasvuruDestekTurId { get; set; }

        public Guid? BasvuruDestekTurGuid { get; set; }

        public string? Ad { get; set; } = null!;

        public string? Aciklama { get; set; }

        public bool? AktifMi { get; set; }

        public bool? SilindiMi { get; set; }
    }

}