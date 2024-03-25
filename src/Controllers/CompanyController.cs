using iBudget.Business;
using iBudget.DAO.Entities;
using iBudget.Models;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class CompanyController : BaseController
{
    private readonly CompanyBusiness _business;

    public CompanyController(CompanyBusiness companyBusiness)
    {
        _business = companyBusiness;
    }

    public async Task<IActionResult> Manipulate()
    {
        try
        {
            var existentCompany = await _business.GetAllAsync();
            return existentCompany != null
                ? RedirectToAction(nameof(Update), new { id = existentCompany.CompanyId })
                : (IActionResult)RedirectToAction(nameof(Create));
        }
        catch (Exception exception)
        {
            if (exception is ECompanyBusinessException companyException)
                return RedirectToAction(nameof(Create));
            throw;
        }
    }

    public IActionResult Create()
    {
        return View(new CompanyModel());
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Create(CompanyModel company)
    {
        if (company == null)
            return BadRequest(ModelState);

        if (company.FormImageFile != null)
            company.SetNewImage();

        if (!ModelState.IsValid)
            return View(company);

        await _business.AddAsync(company);
        return Redirect("/");
    }

    public async Task<IActionResult> Update(int id)
    {
        var company = await _business.GetByIdAsync(id);
        return View(company);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Update(CompanyModel company)
    {
        if (SystemManager.IsDevelopment)
        {
            ViewBag.Message = "Em modo desenvolvimento não é possível alterar a empresa!";
            return View(company);
        }

        if (company == null)
            return BadRequest(ModelState);

        if (company.FormImageFile != null)
            company.SetNewImage();

        if (!ModelState.IsValid)
            return View(company);

        await _business.UpdateAsync(company);
        return Redirect("/");
    }
}