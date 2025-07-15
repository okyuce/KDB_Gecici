using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirListeTebligatMalikler;

public class GetirListeTebligatMaliklerQueryResponseModel
{

    public long? BasvuruKamuUstlenecekId { get; set; }

    public Guid? BasvuruKamuUstlenecekGuid { get; set; }

    public long? BasvuruDurumId { get; set; }

    public string? BasvuruDurumAd { get; set; }

    public string? AskiKodu { get; set; }
    public string? HasarTespitHasarDurumu { get; set; }

    public string? TcKimlikNo { get; set; } = null!;

    public string? TcKimlikNoRaw { get; set; } = null!;

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? CepTelefonu { get; set; }

    public string? Eposta { get; set; }

    public int? TapuTasinmazId { get; set; }

    public int? TapuAnaTasinmazId { get; set; }

    public int? TapuIlId { get; set; }

    public string? TapuIlAdi { get; set; }

    public int? TapuIlceId { get; set; }

    public string? TapuIlceAdi { get; set; }

    public int? TapuMahalleId { get; set; }

    public string? TapuMahalleAdi { get; set; }

    public string? TapuAda { get; set; }

    public string? TapuParsel { get; set; }

    public string? TapuTasinmazTipi { get; set; }

    public string? TapuBagimsizBolumNo { get; set; }

    public int? TapuKat { get; set; }

    public string? TapuBlok { get; set; }

    public string? TapuGirisBilgisi { get; set; }

    public string? TapuNitelik { get; set; }

    public string? TapuRehinDurumu { get; set; }

    public int? TapuIstirakNo { get; set; }

    public string? TuzelKisiVergiNo { get; set; }

    public string? TuzelKisiAdi { get; set; }

    public string? TuzelKisiMersisNo { get; set; }

    public string? TuzelKisiAdres { get; set; }

    public string? TuzelKisiYetkiTuru { get; set; }

    public int? TuzelKisiTipId { get; set; }

    public long? BinaDegerlendirmeId { get; set; }

    public long? BasvuruId { get; set; }

    public long? TapuArsaPay { get; set; }

    public long? TapuArsaPayda { get; set; }
    
    public bool PasifMalikMi { get; set; } = false;

    public long? IstirakTebligatGonderimDetayId { get; set; }

    public long? TeslimTebligatGonderimDetayId { get; set; }
}