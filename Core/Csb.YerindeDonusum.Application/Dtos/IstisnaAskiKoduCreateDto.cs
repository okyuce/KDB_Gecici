using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Dtos
{
    public class IstisnaAskiKoduCreateDto
    {
        public string AskiKodu { get; set; }
        public bool AktifMi { get; set; }
        public bool SilindiMi { get; set; }
        public string OlusturanIp { get; set; }
        public long OlusturanKullaniciId { get; set; }
    }
}
