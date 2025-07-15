using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Models.Mail
{
    public class MailOptionModel
    {
        public required string Domain { get; set; }
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required bool UseDefaultCredentials { get; set; }
        public required bool EnableSsl { get; set; }
        public required int TimeoutSeconds { get; set; }
        public required string FromEmail { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
