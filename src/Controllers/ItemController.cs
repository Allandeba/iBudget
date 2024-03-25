using iBudget.Business;
using iBudget.DAO.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace iBudget.Controllers;

public class ItemController : BaseController
{
    private readonly ItemBusiness _business;

    public ItemController(ItemBusiness itemBusiness)
    {
        _business = itemBusiness;
    }

    public async Task<IActionResult> Index(int? pageNumber)
    {
        var items = await _business.GetItemsPagination(pageNumber);
        return View(items);
    }

    public IActionResult Create()
    {
        return View(new ItemModel());
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Create(ItemModel item)
    {
        if (item == null) return BadRequest(ModelState);

        item.SetItemImageList();
        item.SetDefaultImage(item.DefaultImage);

        if (!ModelState.IsValid)
            return View(item);

        await _business.AddAsync(item);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var item = await _business.GetByIdAsync(id);
        return View(item);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Update(ItemModel item)
    {
        if (item == null)
            return BadRequest(ModelState);

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

    public async Task<IActionResult> Search(string search, int? pageNumber)
    {
        TempData[Constants.SearchBoxData] = search ?? "";
        var items = await _business.GetAllLikeAsync(search, pageNumber);
        return View(nameof(Index), items);
    }
}