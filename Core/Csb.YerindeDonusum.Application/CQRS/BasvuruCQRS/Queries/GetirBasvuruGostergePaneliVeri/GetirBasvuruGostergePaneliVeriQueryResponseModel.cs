namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruGostergePaneliVeri;

public class GetirBasvuruGostergePaneliVeriQueryResponseModel
{
    public int ToplamBasvuruSayisi { get; set; }
    public int AktifBasvuruSayisi { get; set; }
    public int HibeKrediBasvuruSayisi { get; set; }
    public int HibeBasvuruSayisi { get; set; }
    public int KrediBasvuruSayisi { get; set; }
    public List<GetirBasvuruGostergePaneliVeriBasvuruTarihModel> BasvuruSayiListe { get; set; }
}