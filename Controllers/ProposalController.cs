﻿using getQuote.Business;
using getQuote.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace getQuote.Controllers
{
    public class ProposalController : BaseController
    {
        private readonly ProposalBusiness _business;

        public ProposalController(ProposalBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProposalModel> proposals = await _business.GetProposals();
            return View(proposals);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateViewBagDefault();
            return View();
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
            ProposalModel? proposal = await _business.GetByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            ;

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

        public async Task<IActionResult> Print(Guid id)
        {
            ViewBag.Company = await _business.GetCompany();
            ProposalModel proposal = await _business.GetPrintByGUIDAsync(id);
            return View(proposal);
        }

        public IActionResult ExportToPDF(Guid id)
        {
            string url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            url = $"{url}/{ControllerContext.RouteData.Values["controller"]}/{nameof(Print)}/{id}";

            ExportToPDFModel exportPDF = new();
            return File(
                exportPDF.GetPDF(url),
                System.Net.Mime.MediaTypeNames.Application.Pdf,
                $"Proposal.pdf"
            );
        }

        public async Task<IActionResult> SendWhatsApp(Guid id)
        {
            ProposalModel? proposal = await _business.GetByGUIDAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            ;

            CompanyModel company = await _business.GetCompany();
            if (company == null)
            {
                return NotFound();
            }
            ;

            string url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            url =
                $"{url}/{ControllerContext.RouteData.Values["controller"]}/{nameof(ExportToPDF)}/{id}";

            string? number = proposal.Person?.Contact.Phone;
            string msg = string.Format(Messages.WhatsAppMessage, company.CompanyName, url);

            string encodedMessage =
                Constants.WhatsAppURL
                + HttpUtility.UrlEncode(number)
                + "&text="
                + HttpUtility.UrlEncode(msg);

            return Redirect(encodedMessage);
        }

        public async Task<IActionResult> Search(string? search)
        {
            TempData[Constants.SearchBoxData] = search ?? "";
            IEnumerable<ProposalModel>? proposals = await _business.GetAllLikeAsync(search);
            return View(nameof(Index), proposals);
        }

        private async Task PopulateViewBagUpdate(ProposalModel proposal)
        {
            ViewBag.ProposalPerson = proposal.Person;
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
}
