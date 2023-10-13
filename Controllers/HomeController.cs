using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
