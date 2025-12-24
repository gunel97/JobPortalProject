using JobPortalProject.BL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.UserMvc.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IResumePdfService _resumePdfService;

        public ResumeController(IResumePdfService resumePdfService)
        {
            _resumePdfService = resumePdfService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id, int languageId = 1)
        {
            try
            {
                var pdfBytes = await _resumePdfService.GenerateResumePdfAsync(id, languageId);

                var fileName = $"Resume_{id}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while generating the PDF.";
                return RedirectToAction("Index");
            }
        }

        // Action to preview PDF in browser
        [HttpGet]
        public async Task<IActionResult> PreviewPdf(int id, int languageId)
        {
            try
            {
                var pdfBytes = await _resumePdfService.GenerateResumePdfAsync(id, languageId);
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while generating the PDF.";
                return RedirectToAction("Index");
            }
        }

    }
}
