using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Dtos
{
    public class IstisnaAskiKoduDosyaUpdateDto : IstisnaAskiKoduDosyaCreateDto
    {
        public long IstisnaAskiKoduDosyaId { get; set; }
        public string GuncelleyenIp { get; set; }
        public long GuncelleyenKullaniciId { get; set; }
    }
}
