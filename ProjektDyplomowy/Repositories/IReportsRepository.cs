using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models;

namespace ProjektDyplomowy.Repositories
{
    public interface IReportsRepository : IBaseRepository<Report>
    {
        Task<PagedReportsViewModel> GetPagedReportsAsync(ReportStatus reportStatus = ReportStatus.Oczekujący, int page = 1);
        Task<List<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(Guid reportId);
    }
}
