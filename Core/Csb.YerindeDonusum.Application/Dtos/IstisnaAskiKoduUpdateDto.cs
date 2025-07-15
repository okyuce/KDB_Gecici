using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Dtos
{
    public class IstisnaAskiKoduUpdateDto : IstisnaAskiKoduCreateDto
    {
        public long IstisnaAskiKoduId { get; set; }
        public string GuncelleyenIp { get; set; }
        public long GuncelleyenKullaniciId { get; set; }
    }
}
