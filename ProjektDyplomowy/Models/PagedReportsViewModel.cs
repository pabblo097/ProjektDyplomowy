using ProjektDyplomowy.Entities;
using System.Collections.Generic;

namespace ProjektDyplomowy.Models
{
    public class PagedReportsViewModel : PagedResultBase
    {
        public List<Report> Reports { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}
