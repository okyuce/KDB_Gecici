using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Dtos
{
    public class IstisnaAskiKoduDosyaCreateDto
    {
        public long IstisnaAskiKoduId { get; set; }
        public string DosyaAdi { get; set; }
        public string DosyaYolu { get; set; }
        public string DosyaTuru { get; set; }
        public bool AktifMi { get; set; }
        public string OlusturanIp { get; set; }
        public long OlusturanKullaniciId { get; set; }
    }
}
