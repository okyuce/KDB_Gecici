using Csb.YerindeDonusum.Application.CQRS.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class UploadIstisnaAskiKoduDosyaCommand : IRequest<FileUploadResponseModel>
    {
        public long IstisnaAskiKoduId { get; set; }
        public IFormFile Dosya { get; set; }
    }
}
