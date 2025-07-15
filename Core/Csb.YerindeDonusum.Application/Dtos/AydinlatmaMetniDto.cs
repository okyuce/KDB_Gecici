namespace Csb.YerindeDonusum.Application.Dtos;

public class AydinlatmaMetniDto
{
    public Guid Id { get; set; }

    public string Icerik { get; set; } = string.Empty;

    public AydinlatmaMetniDto()
    {
    }
    
    public AydinlatmaMetniDto(Guid id, string icerik)
    {
        Id = id;
        Icerik = icerik;
    }
}