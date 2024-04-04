using iBudget.Models;
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
                Id = "personMenu",
                Title = "Pessoa",
                Url = "Person",
                IconClass = "svg svg-person"
            },
            new()
            {
                Id = "itemMenu",
                Title = "Item",
                Url = "Item",
                IconClass = "svg svg-item"
            },
            new()
            {
                Id = "proposalMenu",
                Title = "Orçamento",
                Url = "Proposal",
                IconClass = "svg svg-proposal"
            },
            new()
            {
                Id = "catalogMenu",
                Title = "Catalogo",
                Url = "Catalog",
                IconClass = "svg svg-catalog"
            },
            new()
            {
                Id = "settingsMenu",
                Title = "Configurações",
                Url = "Settings",
                IconClass = "svg svg-settings"
            }
        };

        return View(menuItems);
    }
}