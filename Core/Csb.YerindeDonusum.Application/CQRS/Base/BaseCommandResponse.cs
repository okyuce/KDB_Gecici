using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.Base
{
    public class BaseCommandResponse
    {
        public bool IsError { get; set; }
        public string ErrorMessageContent { get; set; }
    }
}
