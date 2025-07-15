namespace Csb.YerindeDonusum.Application.Dtos;

public class AyarDto
{
    public AyarBasvuruDto Basvuru { get; set; }
}

public class AyarBasvuruDto {
    public int EnFazlaTicarethaneHibeSayisi { get; set; }
    public int EnFazlaEvHibeSayisi { get; set; }
    public int EnFazlaKrediSayisi { get; set; }
    public int EnFazlaTicarethaneKrediSayisi { get; set; }
    public int EnFazlaEvKrediSayisi { get; set; }
    public int KotUstKatSayisi { get; set; }
}