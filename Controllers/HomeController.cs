using iBudget.Models;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        var menuItems = new List<HomeItemModel>
        {
            new HomeItemModel
            {
                Title = "Pessoa",
                Url = "Person",
                IconClass = "svg svg-person"
            },
            new HomeItemModel
            {
                Title = "Item",
                Url = "Item",
                IconClass = "svg svg-item"
            },
            new HomeItemModel
            {
                Title = "Orçamento",
                Url = "Proposal",
                IconClass = "svg svg-proposal"
            },
            new HomeItemModel
            {
                Title = "Catalogo",
                Url = "Catalog",
                IconClass = "svg svg-catalog"
            },
            new HomeItemModel
            {
                Title = "Configurações",
                Url = "Settings",
                IconClass = "svg svg-settings"
            }
        };

        return View(menuItems);
    }
}
