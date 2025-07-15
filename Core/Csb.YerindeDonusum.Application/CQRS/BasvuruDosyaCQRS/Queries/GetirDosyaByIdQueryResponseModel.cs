namespace Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Queries
{
    public class GetirDosyaByIdQueryResponseModel
    {
        public byte[] File { get; set; }
        public string MimeType { get; set; }
    }
}