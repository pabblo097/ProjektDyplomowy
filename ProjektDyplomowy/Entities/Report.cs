namespace ProjektDyplomowy.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public int ReportCount { get; set; }
    }
}
