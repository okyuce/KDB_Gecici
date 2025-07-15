using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.GuncelleBasvuru;

public class GuncelleBasvuruCommandModel
{
    public string? BasvuruGuid { get; set; }
    public int? BasvuruId { get; set; }
    public string? TcKimlikNo { get; set; }

    public string? UavtAdresNo { get; set; }
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public int? UavtCaddeNo { get; set; }
    public int? UavtMeskenBinaNo { get; set; }
    public string? UavtIcKapiNo { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public string? UavtCsbm { get; set; }

    //public string? UavtCsbmKodu { get; set; }

    public string? UavtDisKapiNo { get; set; }
    public bool? TapuHazineArazisiMi { get; set; }
    public bool? UavtBeyanMi { get; set; }
    public List<DosyaDto>? BasvuruDosyaListe { get; set; }
}