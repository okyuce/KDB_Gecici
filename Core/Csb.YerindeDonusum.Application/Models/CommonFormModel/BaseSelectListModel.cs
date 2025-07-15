namespace Csb.YerindeDonusum.Application.Models.CommonFormModel;

public class BaseSelectListModel<TId> where TId : struct
{
    public TId Id { get; set; }

    public string Ad { get; set; }
}
