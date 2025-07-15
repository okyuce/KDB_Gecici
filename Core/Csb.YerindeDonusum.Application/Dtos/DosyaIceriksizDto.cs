namespace Csb.YerindeDonusum.Application.Dtos;

public class DosyaIceriksizDto
{
    public Guid? Id { get; set; }
    public long? DbId { get; set; }
    public string? DosyaAdi { get; set; }
    public Guid? DosyaTurId { get; set; }
    public long? DosyaTurDbId { get; set; }
    public string? DosyaTurAdi { get; set; }
}