using iBudget.DAO.Entities;
using Microsoft.AspNetCore.Mvc;

namespace iBudget.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        var menuItems = new List<HomeItemModel>
        {
            new()
            {
                Title = "Pessoa",
                Url = "Person",
                IconClass = "svg svg-person"
            },
            new()
            {
                Title = "Item",
                Url = "Item",
                IconClass = "svg svg-item"
            },
            new()
            {
                Title = "Orçamento",
                Url = "Proposal",
                IconClass = "svg svg-proposal"
            },
            new()
            {
                Title = "Catalogo",
                Url = "Catalog",
                IconClass = "svg svg-catalog"
            },
            new()
            {
                Title = "Configurações",
                Url = "Settings",
                IconClass = "svg svg-settings"
            }
        };

        return View(menuItems);
    }
}