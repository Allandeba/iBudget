using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers
{
    public class ConfigurationController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
