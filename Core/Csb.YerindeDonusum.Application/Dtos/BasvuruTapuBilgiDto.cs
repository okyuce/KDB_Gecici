using System.Text.Json.Serialization;

namespace Csb.YerindeDonusum.Application.Dtos;

public class BasvuruTapuBilgiDto
{
    public long Pay { get; set; } //türksat tarafı bu isimde gönderdiği için bizim tarafta property çoklanarak çözüldü
    public long Payda { get; set; } //türksat tarafı bu isimde gönderdiği için bizim tarafta property çoklanarak çözüldü
    public long HissePay { get; set; }
    public long HissePayda { get; set; }
    public string? HisseTuru { get; set; }
    public int? IstirakNo { get; set; }
    public string? TapuMudurlugu { get; set; }
    public string? IslemTanim { get; set; }
    public int? YevmiyeNo { get; set; }
    public DateTime? YevmiheTarihi { get; set; }
}