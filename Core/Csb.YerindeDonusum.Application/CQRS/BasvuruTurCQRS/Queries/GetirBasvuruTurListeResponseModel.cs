using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS.Queries
{
    public class GetirBasvuruTurListeResponseModel
    {
        public long? BasvuruTurId { get; set; }

        public Guid? BasvuruTurGuid { get; set; }

        public string? Ad { get; set; }

        public string? Aciklama { get; set; }

        public bool? AktifMi { get; set; }

        public bool? SilindiMi { get; set; }

        //public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();
    }

}