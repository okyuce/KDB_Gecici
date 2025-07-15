namespace Csb.YerindeDonusum.Application.Dtos.Afad;

public class AfadBasvuruDto
{
    public long Tckn { get; set; }
    public long? mirascisiOlunanTckn { get; set; }
    public long MaksKimlikNo { get; set; }
    public long BasvuruNo { get; set; }
    public string Huid { get; set; }
    public string AdresNo { get; set; }
    public string AskiBaskiDetayAskiKodu { get; set; }
    public string DegerlendirmeDurumu { get; set; }
    public string HasarDurumu { get; set; }
    public string Il { get; set; }
    public string Ilce { get; set; }
    public string Mahalle { get; set; }
    public string Ada { get; set; }
    public string Parsel { get; set; }
    public string Aciklama { get; set; }
    public bool Manuel { get; set; }
    public bool KayitliOlmayanTapuBilgisi { get; set; }
    public bool YapiKayitBelgesiVar { get; set; }
    public bool EkBelgeleriVar { get; set; }
}