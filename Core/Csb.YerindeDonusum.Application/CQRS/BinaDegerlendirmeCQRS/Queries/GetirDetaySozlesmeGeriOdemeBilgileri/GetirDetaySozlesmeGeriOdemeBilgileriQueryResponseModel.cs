using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetaySozlesmeGeriOdemeBilgileri
{
    public class GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel
    {
        public string? TcKimlikNo { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public DateOnly? SozlesmeTarihi { get; set; }
        public long? BinaDegerlendirmeId { get; set; }
        public long? YapiKimlikNo { get; set; }
        public long? IzinBelgeNo { get; set; }
        public decimal? KrediOdemeTutar { get; set; }
    }
}
