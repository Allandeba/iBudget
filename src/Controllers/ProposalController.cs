using System.Net.Mime;
using System.Web;
using iBudget.Business;
using iBudget.DAO.Entities;
using iBudget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class ProposalController : BaseController
{
    private readonly ProposalBusiness _business;

    public ProposalController(ProposalBusiness business)
    {
        _business = business;
    }

    public async Task<IActionResult> Index(int? pageNumber)
    {
        var proposals = await _business.GetProposalsPagination(pageNumber);
        return View(proposals);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateViewBagDefault();
        return View(new ProposalModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProposalModel proposal)
    {
        if (!ModelState.IsValid)
        {
            await PopulateViewBagDefault();
            return View(proposal);
        }

        await _business.AddAsync(proposal);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var proposal = await _business.GetByIdAsync(id);
        if (proposal == null)
            return NotFound();

        await PopulateViewBagUpdate(proposal);
        return View(proposal);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProposalModel proposal)
    {
        if (!ModelState.IsValid)
        {
            await _business.IncludePerson(proposal);
            await _business.IncludeItems(proposal);
            await PopulateViewBagUpdate(proposal);
            return View(proposal);
        }

        await _business.UpdateAsync(proposal);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _business.RemoveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public async Task<IActionResult> Print(Guid id)
    {
        ViewBag.Company = await _business.GetCompany();
        var proposal = await _business.GetPrintByGUIDAsync(id);
        return View(proposal);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExportToPDF(Guid id)
    {
        var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        url = $"{url}/{ControllerContext.RouteData.Values["controller"]}/{nameof(Print)}/{id}";

        var company = await _business.GetCompany();
        if (company == null)
            return NotFound();

        ExportToPDFModel exportPDF = new();
        return File(
            exportPDF.GetPDF(url),
            MediaTypeNames.Application.Pdf,
            "Proposal.pdf"
        );
    }

    public async Task<IActionResult> SendWhatsApp(Guid id)
    {
        var proposal = await _business.GetByGUIDAsync(id);
        if (proposal == null)
            return NotFound();

        var company = await _business.GetCompany();
        if (company == null)
            return NotFound();

        var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        url =
            $"{url}/{ControllerContext.RouteData.Values["controller"]}/{nameof(ExportToPDF)}/{id}";

        var number = proposal.Person?.Contact.Phone;

        var msg = SystemManager.IsDevelopment
            ? string.Format(Messages.WhatsAppMessage, Constants.ApplicationName, url)
            : string.Format(Messages.WhatsAppMessage, company.CompanyName, url);

        var encodedMessage =
            Constants.WhatsAppURL
            + HttpUtility.UrlEncode(number)
            + "&text="
            + HttpUtility.UrlEncode(msg);

        return Redirect(encodedMessage);
    }

    public async Task<IActionResult> Search(string search, int? pageNumber)
    {
        TempData[Constants.SearchBoxData] = search ?? "";
        var proposals = await _business.GetAllLikeAsync(search, pageNumber);
        return View(nameof(Index), proposals);
    }

    private async Task PopulateViewBagUpdate(ProposalModel proposal)
    {
        ViewBag.ProposalContent = proposal.ProposalContent;
        await PopulateViewBagDefault();
    }

    private async Task PopulateViewBagDefault()
    {
        ViewBag.People = await _business.GetSelectListPeople();
        ViewBag.Items = await _business.GetSelectListItems();
        ViewBag.ItemsFull = await _business.GetDynamicItems();
    }
}