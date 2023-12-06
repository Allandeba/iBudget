using iBudget.Business;
using iBudget.Models;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers
{
    public class ProposalHistoryController : BaseController
    {
        private readonly ProposalHistoryBusiness _business;

        public ProposalHistoryController(ProposalHistoryBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Print(int id)
        {
            ViewBag.Company = await _business.GetCompany();
            ProposalModel proposal = await _business.GetProposalFromHistory(id);
            return View(proposal);
        }

        public async Task<IActionResult> ExportToPDF(int id)
        {
            if (SystemManager.IsDevelopment)
                return RedirectToAction(nameof(Print), new { id = id });

            string url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            url = $"{url}/{ControllerContext.RouteData.Values["controller"]}/{nameof(Print)}/{id}";

            CompanyModel company = await _business.GetCompany();
            if (company == null)
                return NotFound();

            ExportToPDFModel exportPDF = new();
            return File(
                exportPDF.GetPDF(url),
                System.Net.Mime.MediaTypeNames.Application.Pdf,
                $"ProposalHistory.pdf"
            );
        }
    }
}
