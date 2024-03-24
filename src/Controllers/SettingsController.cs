using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class SettingsController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}