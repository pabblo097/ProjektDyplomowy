using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.DAL;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models;

namespace ProjektDyplomowy.Repositories
{
    public class ReportsRepository : BaseRepository<Report>, IReportsRepository
    {
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment hostEnvironment;

        public ReportsRepository(AppDbContext context, IMapper mapper, IWebHostEnvironment hostEnvironment) : base(context)
        {
            this.mapper = mapper;
            this.hostEnvironment = hostEnvironment;
        }

        public Task<List<Report>> GetAllReportsAsync()
        {
            return context.Reports.ToListAsync();
        }

        public async Task<PagedReportsViewModel> GetPagedReportsAsync(ReportStatus reportStatus = ReportStatus.Oczekujący, int page = 1)
        {
            var reports = context.Reports.Where(r => r.ReportStatus == reportStatus);

            int size = 30;
            int skip = (page - 1) * size;
            int count = await reports.CountAsync();
            reports = reports.Skip(skip).Take(size);

            var pagedReportsViewModel = new PagedReportsViewModel
            {
                Reports = await reports.ToListAsync(),
                PageSize = size,
                AllItemsCount = count,
                CurrentPage = page,
                ReportStatus = reportStatus
            };

            return pagedReportsViewModel;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage", Justification = "<Oczekujące>")]
        public async Task<Report> GetReportByIdAsync(Guid reportId)
        {
            return await context.Reports.FindAsync(reportId);
        }
    }
}
