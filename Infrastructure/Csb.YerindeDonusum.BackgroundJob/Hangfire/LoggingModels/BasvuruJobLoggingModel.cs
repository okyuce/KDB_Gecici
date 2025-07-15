namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.LoggingModels;
record BasvuruJobLoggingModel
{
    public Guid BasvuruId { get; set; }
    public int? TakbisIlId { get; set; }
    public int? TakbisIlceId { get; set; }
    public int? TakbisMahalleId { get; set; }
    public long GecenMilisaniye { get; set; }    
}
