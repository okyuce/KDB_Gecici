namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries
{
    public class KdsHaneModel
    {
        public bool BasvuruYapabilirMi { get; set; }
        public string? BilgilendirmeMesaji { get; set; }
        public KdsHaneDetayModel Detay { get; set; }
    }

    public class KdsHaneDetayModel
    {
        public string? Uid { get; set; }
        public string? AskiKodu { get; set; }
        public string? HasarDurumu { get; set; }
        public string? ItirazSonucu { get; set; }
        public int? IlKod { get; set; }
        public string? IlAd { get; set; }
        public int? IlceKod { get; set; }
        public string? IlceAd { get; set; }
        public int? MahalleKod { get; set; }
        public string? MahalleAd { get; set; }
        public string? Sokak { get; set; }
        public string? DisKapiNo { get; set; }
        public string? AdaNo { get; set; }
        public string? ParselNo { get; set; }
        public long? uavtkod { get; set; }
        public string? GuclendirmeMahkemeSonucu { get; set; }
    }
}