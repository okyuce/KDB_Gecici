namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirAfadBasvuruById;

public class GetirAfadBasvuruByIdQueryResponseModel
{
    public Guid CsbId { get; set; }

    public long OlayId { get; set; }

    public string? BasvuruTipi { get; set; }

    public string? Tckn { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? EbeveynTckn { get; set; }

    public long? KomisyonKararNo { get; set; }

    public string? HasarDurumu { get; set; }

    public string? KullanimAmaci { get; set; }

    public string? AskiKodu { get; set; }

    public string? Huid { get; set; }

    public long? MaksYapiKimlikNo { get; set; }

    public string? Wkt { get; set; }

    public string? Aciklama { get; set; }

    public string? Il { get; set; }

    public int? IlId { get; set; }

    public string? Ilce { get; set; }

    public int? IlceId { get; set; }

    public string? Mahalle { get; set; }

    public int? MahalleId { get; set; }

    public DateTime? OlusturulmaTarihi { get; set; }

    public string? Telefon { get; set; }

    public string? BabaAd { get; set; }

    public string? TapuIl { get; set; }

    public string? TapuIlce { get; set; }

    public string? TapuMahalle { get; set; }

    public string? Ada { get; set; }

    public string? Parsel { get; set; }

    public long? TasinmazId { get; set; }

    public long? AltTasinmazId { get; set; }

    public string? TasinmazCinsi { get; set; }

    public bool? ItirazOlusmus { get; set; }

    public DateTime? ItirazTarihi { get; set; }

    public long? ItirazId { get; set; }

    public long? ItirazDegerlendirmeId { get; set; }

    public string? ItirazDegerlendirmeSonucu { get; set; }

    public long? ItirazDegerlendirmeSonucuId { get; set; }

    public string? DegerlendirmeIptalDurumu { get; set; }

    public long? BasvuruNo { get; set; }

    public bool? KuraIsabetEttiMi { get; set; }

    public string? DegerlendirmeDurumu { get; set; }
}