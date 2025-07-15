namespace Csb.YerindeDonusum.Application.Models;

public class JwtOptionModel
{
    public double ExpireTimeMinute { get; set; }
    public double? CookieExpireTimeMinute { get; set; }
    public byte[] IssuerSigningKey { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
}