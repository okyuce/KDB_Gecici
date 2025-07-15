using Csb.YerindeDonusum.Application.Models.DataTable;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS;

public class GetirListeSikcaSorulanSoruListeServerSideQueryModel : DataTableModel
{
    public string? Soru { get; set; }

    public bool? AktifMi { get; set; }
}