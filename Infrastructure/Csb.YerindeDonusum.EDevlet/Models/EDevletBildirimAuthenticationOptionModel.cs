namespace Csb.YerindeDonusum.EDevlet.Models;

public class EDevletBildirimAuthenticationOptionModel
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int KurumKodu { get; set; } = -1;
    public string AnahtarTeslimKampanyaId { get; set; } = string.Empty;
    public string IstirakKampanyaId { get; set; } = string.Empty;
}
