using iBudget.Framework;
using iBudget.Models;
using iBudget.Repository;
using System.Linq.Expressions;

namespace iBudget.Business
{
    public class ItemBusiness
    {
        private readonly ItemRepository _repository;

        public ItemBusiness(ItemRepository itemRepository)
        {
            _repository = itemRepository;
        }

        public async Task<IEnumerable<ItemModel>> GetItems()
        {
            ItemIncludes[] includes = new ItemIncludes[] { ItemIncludes.None };
            IEnumerable<ItemModel> items = await _repository.GetAllAsync(
                includes.Cast<Enum>().ToArray()
            );
            return items.OrderByDescending(i => i.ItemId);
        }

        public async Task AddAsync(ItemModel item)
        {
            await _repository.AddAsync(item);
        }

        public async Task<ItemModel> GetByIdAsync(int itemId)
        {
            ItemIncludes[] includes = new ItemIncludes[] { ItemIncludes.ItemImage };
            return await _repository.GetByIdAsync(itemId, includes.Cast<Enum>().ToArray());
        }

        public async Task UpdateAsync(ItemModel item)
        {
            ItemIncludes[] includes = new ItemIncludes[] { ItemIncludes.ItemImage };
            ItemModel existentItem = await _repository.GetByIdAsync(
                item.ItemId,
                includes.Cast<Enum>().ToArray()
            );

            if (existentItem != null)
            {
                updateExistentItemInformation(existentItem, item);
            }

            await _repository.UpdateAsync(existentItem);
        }

        private void updateExistentItemInformation(ItemModel existentItem, ItemModel itemToUpdate)
        {
            existentItem.ItemName = itemToUpdate.ItemName;
            existentItem.Value = itemToUpdate.Value;
            existentItem.Description = itemToUpdate.Description;

            if (itemToUpdate.ItemImageList != null)
            {
                existentItem.ItemImageList?.AddRange(itemToUpdate.ItemImageList);
            }

            existentItem.SetDefaultImage(itemToUpdate.DefaultImage);

            if (itemToUpdate.IdImagesToDelete?.Count > 0)
            {
                deleteItemImage(existentItem, itemToUpdate.IdImagesToDelete);
            }
            ;
        }

        private void deleteItemImage(ItemModel existentItem, List<int> idImagesToDelete)
        {
            foreach (int idItemImage in idImagesToDelete)
            {
                ItemImageModel? itemImage = existentItem.ItemImageList?.Find(
                    im => im.ItemImageId == idItemImage
                );
                if (itemImage != null)
                {
                    _ = (existentItem.ItemImageList?.Remove(itemImage));
                }
                ;
            }
        }

        public async Task RemoveAsync(int itemId)
        {
            ItemIncludes[] includes = new ItemIncludes[] { ItemIncludes.ItemImage };
            ItemModel item = await _repository.GetByIdAsync(
                itemId,
                includes.Cast<Enum>().ToArray()
            );
            if (item == null)
            {
                return;
            }
            ;

            await _repository.RemoveAsync(item);
        }

        public async Task<IEnumerable<ItemModel>> FindAsync(Expression<Func<ItemModel, bool>> where)
        {
            return await _repository.FindAsync(where);
        }

        public async Task<IEnumerable<ItemModel>> FindAsync(
            Expression<Func<ItemModel, bool>> where,
            ItemIncludes[] includes
        )
        {
            return await _repository.FindAsync(where, includes.Cast<Enum>().ToArray());
        }

        public async Task<IEnumerable<ItemModel>> GetAllLikeAsync(string? search)
        {
            return search == null
                ? await GetItems()
                : await _repository.FindAsync(p => p.ItemName.ToLower().Contains(search.ToLower()));
        }

        public async Task IncludeImages(ItemModel item)
        {
            ItemModel itemDB = await GetByIdAsync(item.ItemId);
            item.ItemImageList = itemDB.ItemImageList;
        }
    }
}
