using Microsoft.AspNetCore.Mvc;

namespace getQuote.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
