using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeByTcNo;

public class GetirBasvuruListeByTcNoResponseModel
{
    public bool YeniBasvuruEklenebilirMi { get; set; }
    public string BilgilendirmeMesaji { get; set; }
    public List<GetirBasvuruListeByTcNoDetayModel>? BasvuruListe { get; set; }
}

public class GetirBasvuruListeByTcNoDetayModel
{
    public Guid Id { get; set; }
    public string BasvuruKodu { get; set; }
    public string TcKimlikNo { get; set; } = null!;
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Eposta { get; set; }
    public string BasvuruDurumu { get; set; }
    public string? BasvuruIptalAciklamasi { get; set; }
    public string BasvuruTuru { get; set; }
    public string BasvuruDestekTuru { get; set; }
    public string BasvuruKanali { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? HasarTespitHasarDurumu { get; set; }
    public string? HasarTespitItirazSonucu { get; set; }
    public string? HasarTespitGuclendirmeMahkemeSonucu { get; set; }
    public string? IlAdi { get; set; }
    public string? IlceAdi { get; set; }
    public string? MahalleAdi { get; set; }
    public string? Blok { get; set; }
    public string? OlusturmaTarihi { get; set; }
    public bool IptalEdilebilirMi { get; set; } = false;
    public string? TuzelKisiVergiNumarasi { get; set; }
    public string? TuzelKisiAdi { get; set; }
    public int? TuzelKisiTipId { get; set; }
    //public List<DosyaIceriksizDto> DosyaBasvuruListe { get; set; }
}