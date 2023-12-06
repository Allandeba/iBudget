using iBudget.DAO;
using iBudget.Framework;
using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace iBudget.Repository
{
    public class ItemRepository : IRepository<ItemModel>
    {
        private readonly ApplicationDBContext _context;

        public ItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        private IQueryable<ItemModel> GetQuery(Enum[] includes)
        {
            IQueryable<ItemModel> query = _context.Item;
            foreach (ItemIncludes include in includes)
            {
                switch (include)
                {
                    case ItemIncludes.None:
                        break;

                    case ItemIncludes.ItemImage:
                        query = query.Include(i => i.ItemImageList);
                        break;

                    default:
                        throw new Exception("ItemIncludes type not implemented");
                }
            }

            return query;
        }

        public async Task<ItemModel> GetByIdAsync(int itemId, Enum[] includes)
        {
            IQueryable<ItemModel> query = GetQuery(includes);
            return await query.FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        public async Task<ItemImageModel> GetItemImageByIdAsync(int itemImageId)
        {
            return await _context.ItemImage.FindAsync(itemImageId);
        }

        public async Task<IEnumerable<ItemModel>> GetAllAsync(Enum[] includes)
        {
            IQueryable<ItemModel> query = GetQuery(includes);
            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemModel>> FindAsync(Expression<Func<ItemModel, bool>> where)
        {
            return await _context.Item.Where(where)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemModel>> FindAsync(
            Expression<Func<ItemModel, bool>> where,
            Enum[] includes
        )
        {
            IQueryable<ItemModel> query = GetQuery(includes);
            return await query.Where(where)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(ItemModel item)
        {
            _ = await _context.Item.AddAsync(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ItemModel item)
        {
            _ = _context.Item.Update(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ItemModel item)
        {
            _ = _context.Item.Remove(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task RemoveItemImageAsync(ItemImageModel itemImage)
        {
            _ = _context.ItemImage.Remove(itemImage);
            _ = await _context.SaveChangesAsync();
        }
    }
}
