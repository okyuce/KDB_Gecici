namespace Csb.YerindeDonusum.Application.Models.Sms
{
    public class SmsOptionModel
    {
        public string ServiceUserName { get; set; } = string.Empty;
        public string ServicePassword { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool UseTestProviders { get; set; }
    }
}
