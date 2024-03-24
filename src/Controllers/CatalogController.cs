using iBudget.Business;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class CatalogController : BaseController
{
    private readonly CatalogBusiness _business;

    public CatalogController(CatalogBusiness catalogBusiness)
    {
        _business = catalogBusiness;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _business.GetItems();
        return View(items);
    }
}