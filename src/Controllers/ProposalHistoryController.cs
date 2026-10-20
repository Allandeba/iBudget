using System.Net.Mime;
using iBudget.Business;
using iBudget.Interfaces;
using iBudget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class ProposalHistoryController : BaseController
{
    private readonly ProposalHistoryBusiness _business;
    private readonly IPdfRenderer _pdfRenderer;

    public ProposalHistoryController(ProposalHistoryBusiness business, IPdfRenderer pdfRenderer)
    {
        _business = business;
        _pdfRenderer = pdfRenderer;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Print(int id, bool pdf = false)
    {
        ViewBag.ShowPdfWatermark = pdf && SystemManager.IsDevelopment;
        ViewBag.Company = await _business.GetCompany();
        var proposal = await _business.GetProposalFromHistory(id);
        return View(proposal);
    }

    public async Task<IActionResult> ExportToPDF(int id)
    {
        var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        url = $"{url}/{ControllerContext.RouteData.Values["controller"]}/{nameof(Print)}/{id}?pdf=true";

        var company = await _business.GetCompany();
        if (company == null)
            return NotFound();

        return File(
            await _pdfRenderer.RenderAsync(url, HttpContext.RequestAborted),
            MediaTypeNames.Application.Pdf,
            "ProposalHistory.pdf"
        );
    }
}
