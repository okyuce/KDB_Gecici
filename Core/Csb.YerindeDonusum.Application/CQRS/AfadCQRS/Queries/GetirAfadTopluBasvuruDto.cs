using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries;

public class GetirAfadTopluBasvuruDto
{
    [JsonPropertyName("olayId")]
    [JsonProperty("olayId")]
    public int? OlayId { get; set; }

    [JsonPropertyName("basvuruTipi")]
    [JsonProperty("basvuruTipi")]
    public string? BasvuruTipi { get; set; }

    [JsonPropertyName("tckn")]
    [JsonProperty("tckn")]
    public long? Tckn { get; set; }

    [JsonPropertyName("ad")]
    [JsonProperty("ad")]
    public string? Ad { get; set; }

    [JsonPropertyName("soyad")]
    [JsonProperty("soyad")]
    public string? Soyad { get; set; }

    [JsonPropertyName("ebeveynTckn")]
    [JsonProperty("ebeveynTckn")]
    public long? EbeveynTckn { get; set; }

    [JsonPropertyName("komisyonKararNo")]
    [JsonProperty("komisyonKararNo")]
    public long? KomisyonKararNo { get; set; }

    [JsonPropertyName("hasarDurumu")]
    [JsonProperty("hasarDurumu")]
    public string? HasarDurumu { get; set; }

    [JsonPropertyName("kullanimAmaci")]
    [JsonProperty("kullanimAmaci")]
    public string? KullanimAmaci { get; set; }

    [JsonPropertyName("askiKodu")]
    [JsonProperty("askiKodu")]
    public string? AskiKodu { get; set; }

    [JsonPropertyName("huid")]
    [JsonProperty("huid")]
    public string? Huid { get; set; }

    [JsonPropertyName("maks_yapi_kimlik_no")]
    [JsonProperty("maks_yapi_kimlik_no")]
    public long? MaksYapiKimlikNo { get; set; }

    [JsonPropertyName("wkt")]
    [JsonProperty("wkt")]
    public string? Wkt { get; set; }

    [JsonPropertyName("aciklama")]
    [JsonProperty("aciklama")]
    public string? Aciklama { get; set; }

    [JsonPropertyName("il")]
    [JsonProperty("il")]
    public string? Il { get; set; }

    [JsonPropertyName("ilId")]
    [JsonProperty("ilId")]
    public int? IlId { get; set; }

    [JsonPropertyName("ilce")]
    [JsonProperty("ilce")]
    public string? Ilce { get; set; }

    [JsonPropertyName("ilceId")]
    [JsonProperty("ilceId")]
    public int? IlceId { get; set; }

    [JsonPropertyName("mahalle")]
    [JsonProperty("mahalle")]
    public string? Mahalle { get; set; }

    [JsonPropertyName("mahalleId")]
    [JsonProperty("mahalleId")]
    public int? MahalleId { get; set; }

    [JsonPropertyName("olusturulmaTarihi")]
    [JsonProperty("olusturulmaTarihi")]
    public DateTime? OlusturulmaTarihi { get; set; }

    [JsonPropertyName("telefon")]
    [JsonProperty("telefon")]
    public string? Telefon { get; set; }

    [JsonPropertyName("babaAd")]
    [JsonProperty("babaAd")]
    public string? BabaAd { get; set; }

    [JsonPropertyName("tapuil")]
    [JsonProperty("tapuil")]
    public string? TapuIl { get; set; }

    [JsonPropertyName("tapuilce")]
    [JsonProperty("tapuilce")]
    public string? TapuIlce { get; set; }

    [JsonPropertyName("tapumahalle")]
    [JsonProperty("tapumahalle")]
    public string? TapuMahalle { get; set; }

    [JsonPropertyName("ada")]
    [JsonProperty("ada")]
    public string? Ada { get; set; }

    [JsonPropertyName("parsel")]
    [JsonProperty("parsel")]
    public string? Parsel { get; set; }

    [JsonPropertyName("tasinmaz_id")]
    [JsonProperty("tasinmaz_id")]
    public long? TasinmazId { get; set; }

    [JsonPropertyName("alt_tasinmaz_id")]
    [JsonProperty("alt_tasinmaz_id")]
    public long? AltTasinmazId { get; set; }

    [JsonPropertyName("tasinmaz_cinsi")]
    [JsonProperty("tasinmaz_cinsi")]
    public string? TasinmazCinsi { get; set; }

    [JsonPropertyName("itiraz_olusmus")]
    [JsonProperty("itiraz_olusmus")]
    public bool? ItirazOlusmus { get; set; }

    [JsonPropertyName("itiraz_tarihi")]
    [JsonProperty("itiraz_tarihi")]
    public DateTime? ItirazTarihi { get; set; }

    [JsonPropertyName("itiraz_id")]
    [JsonProperty("itiraz_id")]
    public long? ItirazId { get; set; }

    [JsonPropertyName("itiraz_degerlendirme_id")]
    [JsonProperty("itiraz_degerlendirme_id")]
    public long? ItirazDegerlendirmeId { get; set; }

    [JsonPropertyName("itiraz_degerlendirme_sonucu")]
    [JsonProperty("itiraz_degerlendirme_sonucu")]
    public string? ItirazDegerlendirmeSonucu { get; set; }

    [JsonPropertyName("itiraz_degerlendirme_sonucu_id")]
    [JsonProperty("itiraz_degerlendirme_sonucu_id")]
    public long? ItirazDegerlendirmeSonucuId { get; set; }

    [JsonPropertyName("degerlendirme_iptal_durumu")]
    [JsonProperty("degerlendirme_iptal_durumu")]
    public string? DegerlendirmeIptalDurumu { get; set; }

    [JsonPropertyName("basvuruNo")]
    [JsonProperty("basvuruNo")]
    public long? BasvuruNo { get; set; }

    [JsonPropertyName("kuraIsabetEttiMi")]
    [JsonProperty("kuraIsabetEttiMi")]
    public bool? KuraIsabetEttiMi { get; set; }

    [JsonPropertyName("degerlendirmeDurumu")]
    [JsonProperty("degerlendirmeDurumu")]
    public string? DegerlendirmeDurumu { get; set; }
}