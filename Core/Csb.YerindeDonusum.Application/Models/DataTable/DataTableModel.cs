namespace Csb.YerindeDonusum.Application.Models.DataTable;

public class DataTableModel
{
    public int draw { get; set; }
    public int start { get; set; }
    public int length { get; set; }
    public List<DataTableColumnModel>? columns { get; set; }
    public DataTableSearchModel? search { get; set; }
    public List<DataTableOrderModel>? order { get; set; }
}

public class DataTableColumnModel
{
    public string? data { get; set; }
    public string? name { get; set; }
    public bool? searchable { get; set; }
    public bool? orderable { get; set; }
    public DataTableSearchModel? search { get; set; }
}

public class DataTableSearchModel
{
    public string? value { get; set; }
    public string? regex { get; set; }
}

public class DataTableOrderModel
{
    public int? column { get; set; }
    public string? dir { get; set; }
}