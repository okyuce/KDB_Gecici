namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.GuncelleBasvuru;

public class GuncelleBasvuruCommandResponseModel
{
    public string? Mesaj { get; set; }

    public Guid? BasvuruGuid { get; set; }

    public string? BasvuruKodu { get; set; }
}