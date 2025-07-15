using Csb.YerindeDonusum.Domain.Common;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class SystemConfiguration : BaseEntity
{
    public string? SystemTitle { get; set; }
    public string? Logo { get; set; }
}