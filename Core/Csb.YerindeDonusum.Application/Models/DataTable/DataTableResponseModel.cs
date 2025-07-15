﻿namespace Csb.YerindeDonusum.Application.Models.DataTable;

public class DataTableResponseModel<T> where T : class
{
    public int draw { get; set; }
    public int recordsTotal { get; set; }
    public int recordsFiltered { get; set; }
    public T data { get; set; }
}