using System.Linq.Expressions;
using iBudget.DAO.Entities;
using Shared.Extensions;
using iBudget.Repository;
using Microsoft.EntityFrameworkCore;
using Shared;
using X.PagedList;

namespace iBudget.Business;

public class ItemBusiness
{
    private readonly ItemRepository _repository;

    public ItemBusiness(ItemRepository itemRepository)
    {
        _repository = itemRepository;
    }

    public async Task<IEnumerable<ItemModel>> GetItems()
    {
        ItemIncludes[] includes = { ItemIncludes.None };
        IEnumerable<ItemModel> items = await _repository
            .GetAll(includes.Cast<Enum>().ToArray())
            .OrderByDescending(i => i.ItemId)
            .ToListAsync();
        return items;
    }

    public async Task<IPagedList<ItemModel>> GetItemsPagination(
        int? pageNumber = Constants.InitialPageForPagination
    )
    {
        ItemIncludes[] includes = { ItemIncludes.None };
        var items = await _repository
            .GetAll(includes.Cast<Enum>().ToArray())
            .OrderByDescending(i => i.ItemId)
            .ToPagedListAsync(pageNumber, Constants.QtRegistersPagination);
        return items;
    }

    public async Task AddAsync(ItemModel item)
    {
        await _repository.AddAsync(item);
    }

    public async Task<ItemModel> GetByIdAsync(int itemId)
    {
        ItemIncludes[] includes = { ItemIncludes.ItemImage };
        return await _repository.GetByIdAsync(itemId, includes.Cast<Enum>().ToArray());
    }

    public async Task UpdateAsync(ItemModel item)
    {
        ItemIncludes[] includes = { ItemIncludes.ItemImage };
        var existentItem = await _repository.GetByIdAsync(
            item.ItemId,
            includes.Cast<Enum>().ToArray()
        );

        if (existentItem != null)
            UpdateExistentItemInformation(existentItem, item);

        await _repository.UpdateAsync(existentItem);
    }

    private void UpdateExistentItemInformation(ItemModel existentItem, ItemModel itemToUpdate)
    {
        existentItem.ItemName = itemToUpdate.ItemName;
        existentItem.Value = itemToUpdate.Value;
        existentItem.Description = itemToUpdate.Description;

        if (itemToUpdate.ItemImageList != null) existentItem.ItemImageList?.AddRange(itemToUpdate.ItemImageList);

        existentItem.SetDefaultImage(itemToUpdate.DefaultImage);

        if (itemToUpdate.IdImagesToDelete?.Count > 0)
            DeleteItemImage(existentItem, itemToUpdate.IdImagesToDelete);
    }

    private void DeleteItemImage(ItemModel existentItem, List<int> idImagesToDelete)
    {
        foreach (var idItemImage in idImagesToDelete)
        {
            var itemImage = existentItem.ItemImageList?.Find(
                im => im.ItemImageId == idItemImage
            );
            if (itemImage != null)
                _ = existentItem.ItemImageList?.Remove(itemImage);
        }
    }

    public async Task RemoveAsync(int itemId)
    {
        ItemIncludes[] includes = { ItemIncludes.ItemImage };
        var item = await _repository.GetByIdAsync(
            itemId,
            includes.Cast<Enum>().ToArray()
        );
        if (item == null)
            return;

        await _repository.RemoveAsync(item);
    }

    public async Task<IEnumerable<ItemModel>> FindAsync(Expression<Func<ItemModel, bool>> where)
    {
        return await _repository.Find(where).ToListAsync();
    }

    public async Task<IEnumerable<ItemModel>> FindAsync(
        Expression<Func<ItemModel, bool>> where,
        ItemIncludes[] includes
    )
    {
        return await _repository.FindAsync(where, includes.Cast<Enum>().ToArray());
    }

    public async Task<IPagedList<ItemModel>> GetAllLikeAsync(
        string search,
        int? pageNumber = Constants.InitialPageForPagination
    )
    {
        return string.IsNullOrEmpty(search)
            ? await GetItemsPagination(pageNumber)
            : await _repository
                .Find(p => EF.Functions.ILike(p.ItemName, $"%{search.Unaccent()}%"))
                .ToPagedListAsync(pageNumber, Constants.QtRegistersPagination);
    }

    public async Task IncludeImages(ItemModel item)
    {
        var itemDB = await GetByIdAsync(item.ItemId);
        item.ItemImageList = itemDB.ItemImageList;
    }
}