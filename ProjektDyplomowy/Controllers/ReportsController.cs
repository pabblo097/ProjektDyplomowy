using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Repositories;

namespace ProjektDyplomowy.Controllers
{
    [Controller]
    public class ReportsController : Controller
    {
        private readonly IReportsRepository reportsRepository;
        private readonly IPostsRepository postsRepository;

        public ReportsController(IReportsRepository reportsRepository, IPostsRepository postsRepository)
        {
            this.reportsRepository = reportsRepository;
            this.postsRepository = postsRepository;
        }

        [Route("Report/{postId}")]
        public IActionResult Add(Guid postId, string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new Report { PostId = postId });
        }

        [ValidateReCaptcha]
        [ValidateAntiForgeryToken]
        [HttpPost("Report/{postId}")]
        public async Task<IActionResult> Add(Report model)
        {
            if (ModelState.IsValid)
            {
                var reports = await reportsRepository.GetAllReportsAsync();

                foreach (var report in reports)
                {
                    if (report.PostId == model.PostId)
                    {
                        report.ReportCount++;
                        report.ReportStatus = ReportStatus.Oczekujący;
                        await reportsRepository.UpdateAsync(report);
                        TempData["SuccessAlert"] = "Pomyślnie zgłoszono post.";
                        return RedirectToAction("Details", "Posts", new { postId = model.PostId });
                    }
                }


                model.Id = Guid.NewGuid();
                model.ReportStatus = ReportStatus.Oczekujący;
                model.ReportCount = 1;

                await reportsRepository.AddAsync(model);

                TempData["SuccessAlert"] = "Pomyślnie zgłoszono post.";
                return RedirectToAction("Details", "Posts", new { postId = model.PostId });
            }
            else
            {
                ModelState.AddModelError("Captcha", "Potwierdź, że nie jesteś robotem.");
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Manage(ReportStatus reportStatus = ReportStatus.Oczekujący, int page = 1)
        {
            var pagedReports = await reportsRepository.GetPagedReportsAsync(reportStatus, page);

            return View(pagedReports);
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost("[controller]/[action]/{reportId}")]
        public async Task<IActionResult> Reject(Guid reportId)
        {
            var report = await reportsRepository.GetReportByIdAsync(reportId);

            if (report == null)
                return RedirectToAction("Error404", "Error");

            report.ReportStatus = ReportStatus.Odrzucony;

            await reportsRepository.UpdateAsync(report);

            TempData["SuccessAlert"] = "Pomyślnie odrzucono zgłoszenie.";
            return RedirectToAction("Manage", "Reports");
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost("[controller]/[action]/{reportId}")]
        public async Task<IActionResult> Approve(Guid reportId)
        {
            var report = await reportsRepository.GetReportByIdAsync(reportId);

            if (report == null)
                return RedirectToAction("Error404", "Error");

            var post = await postsRepository.GetPostByIdAsync(report.PostId);

            if (post == null)
                return RedirectToAction("Error404", "Error");

            report.ReportStatus = ReportStatus.Zatwierdzony;

            await reportsRepository.UpdateAsync(report);
            await postsRepository.RemoveAsync(post);

            TempData["SuccessAlert"] = "Pomyślnie zatwierdzono zgłoszenie i usunięto post.";
            return RedirectToAction("Manage", "Reports");
        }
    }
}
