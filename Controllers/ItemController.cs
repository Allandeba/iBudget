using iBudget.Business;
using iBudget.Controllers;
using iBudget.Models;
using Microsoft.AspNetCore.Mvc;

public class ItemController : BaseController
{
    private readonly ItemBusiness _business;

    public ItemController(ItemBusiness itemBusiness)
    {
        _business = itemBusiness;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<ItemModel> items = await _business.GetItems();
        return View(items);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Create(ItemModel item)
    {
        if (item == null)
        {
            return BadRequest(ModelState);
        }

        item.SetItemImageList();
        item.SetDefaultImage(item.DefaultImage);

        if (!ModelState.IsValid)
        {
            return View(item);
        }

        await _business.AddAsync(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        ItemModel item = await _business.GetByIdAsync(id);
        return View(item);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Update(ItemModel item)
    {
        if (item == null)
        {
            return BadRequest(ModelState);
        }

        item.SetItemImageList();

        if (!ModelState.IsValid)
        {
            await _business.IncludeImages(item);
            return View(item);
        }

        await _business.UpdateAsync(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _business.RemoveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Search(string search)
    {
        TempData[Constants.SearchBoxData] = search ?? "";
        IEnumerable<ItemModel> items = await _business.GetAllLikeAsync(search);
        return View(nameof(Index), items);
    }
}
