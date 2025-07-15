using Csb.YerindeDonusum.Application.Models.DataTable;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS;

public class GetirListeKullaniciQueryModel : DataTableModel
{
    public bool? AktifMi { get; set; }
}