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
                Id = "PersonMenu",
                Title = "Pessoa",
                Url = "Person",
                IconClass = "svg svg-person"
            },
            new()
            {
                Id = "ItemMenu",
                Title = "Item",
                Url = "Item",
                IconClass = "svg svg-item"
            },
            new()
            {
                Id = "ProposalMenu",
                Title = "Orçamento",
                Url = "Proposal",
                IconClass = "svg svg-proposal"
            },
            new()
            {
                Id = "CatalogMenu",
                Title = "Catalogo",
                Url = "Catalog",
                IconClass = "svg svg-catalog"
            },
            new()
            {
                Id = "SettingsMenu",
                Title = "Configurações",
                Url = "Settings",
                IconClass = "svg svg-settings"
            }
        };

        return View(menuItems);
    }
}