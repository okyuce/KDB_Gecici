using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class HasartespitTespitVeri
{
    /// <summary>
    /// tespıt uıd bılgısı
    /// </summary>
    public string Uid { get; set; } = null!;

    public int? IlKod { get; set; }

    public string? IlAd { get; set; }

    public int? IlceKod { get; set; }

    public string? IlceAd { get; set; }

    public int? MahalleKod { get; set; }

    public string? MahalleAd { get; set; }

    public string? Sokak { get; set; }

    public string? DisKapiNo { get; set; }

    public string? YapiKimlikNo { get; set; }

    public int? HasarDurumuId { get; set; }

    public string? HasarDurumu { get; set; }

    public int? ItirazSonucuId { get; set; }

    public int? AfetGrupId { get; set; }

    public int? AfetId { get; set; }

    public DateTime? AfetTarih { get; set; }

    public string? AfetTanim { get; set; }

    public string? AskiKodu { get; set; }

    public int? KatAdedi { get; set; }

    public int? KonutSayisi { get; set; }

    public int? TicarethaneSayisi { get; set; }

    public int? AhirSayisi { get; set; }

    public int? SamanlikSayisi { get; set; }

    public string? KisiKimlik { get; set; }

    public string? Aciklama { get; set; }

    public string? KullanimAmaci { get; set; }

    public bool? YayinlansinMi { get; set; }

    /// <summary>
    /// Yapi içerisindeki nüfus verisinin nvi verisi ile eşleşme sonucu elde edilmiş nüfüs verisi
    /// </summary>
    public int YapiNufus { get; set; }

    public string? HasarDurumuRenkKod { get; set; }

    public long? TapuKimlikNo { get; set; }

    public long? TapuZeminRef { get; set; }

    public string? AdaNo { get; set; }

    public string? ParselNo { get; set; }

    public string? TapuAlani { get; set; }

    public string? ItirazSonucu { get; set; }

    public string? ZeminTipi { get; set; }

    public DateTime? VeriTarihi { get; set; }

    public DateTime? SonTespitZamani { get; set; }

    public long? Uavtkod { get; set; }

    public double? TabanAlani { get; set; }

    public double? HacimMolozM3 { get; set; }

    public int? YerdenYuksekKatSayisi { get; set; }

    public double? HacimM2 { get; set; }

    public int? DoluKonutSayisi { get; set; }

    public bool? YapiKullanimIzin { get; set; }

    public string? BinaKodu { get; set; }

    public DateTime? HafriyatIslemTarihi { get; set; }

    public bool? HafriyatIslemBugun { get; set; }

    public bool? SehirMerkezi { get; set; }

    public int? MaksMahalleKod { get; set; }

    public int? Alan1 { get; set; }

    public int? Alan2 { get; set; }

    public int? Alan3 { get; set; }

    public int? Alan4 { get; set; }

    public int? Alan5 { get; set; }

    public int? Alan6 { get; set; }

    public DateTime? EtlDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? GuclendirmeMahkemeSonucu { get; set; }

    public int? GuclendirmeMahkemeSonucuId { get; set; }
}
