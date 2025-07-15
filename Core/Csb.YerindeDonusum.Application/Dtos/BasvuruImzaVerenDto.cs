using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Dtos
{
    public class BasvuruImzaVerenDto
    {
        public long? BasvuruImzaVerenId { get; set; }
        public string? TcKimlikNo { get; set; }
        public string? AdSoyad { get; set; }
        public long? BasvuruDestekTurId { get; set; }
        public long? BasvuruId { get; set; }
        public Guid? BasvuruGuid { get; set; }
        public long? BasvuruKamuUstlenecekId { get; set; }
        public Guid? BasvuruKamuUstlenecekGuid { get; set; }
        public long? BasvuruTurId { get; set; }
        public long? BasvuruTurIdOrjinal { get; set; }
        public int? HibeOdemeTutar { get; set; }
        public int? KrediOdemeTutar { get; set; }
        public double? BagimsizBolumAlani { get; set; }
        public string? BagimsizBolumNo { get; set; }
        public DateOnly? SozlesmeTarihi { get; set; }
        public string? IbanNo { get; set; }
        public bool IbanGirildiMi { get; set; }
        public int? HissePay { get; set; }
        public int? HissePayda { get; set; }
        public bool? TasinmazinAfadBasvurusuVarMi { get; set; }
        public List<DosyaIceriksizDto>? BasvuruImzaVerenDosyas { get; set; }
        public string? TuzelKisiVergiNo { get; set; }
        public string? TuzelKisiAdi { get; set; }
        public string? TuzelKisiMersisNo { get; set; }
        public string? TuzelKisiAdres { get; set; }
        public string? TuzelKisiYetkiTuru { get; set; }
        public int? TuzelKisiTipId { get; set; }
    }
}
