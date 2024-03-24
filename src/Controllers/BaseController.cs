using System.Diagnostics;
using iBudget.DAO.Entities;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class BaseController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        ErrorViewModel errorViewModel =
            new() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        return View(errorViewModel);
    }
}