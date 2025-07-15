namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;

public class EkleBasvuruCommandResponseModel
{
    public string Mesaj { get; set; }

    public Guid? BasvuruId { get; set; }

    public string? BasvuruKodu { get; set; }
}