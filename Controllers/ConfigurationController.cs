using Microsoft.AspNetCore.Mvc;

namespace getQuote.Controllers
{
    public class ConfigurationController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
