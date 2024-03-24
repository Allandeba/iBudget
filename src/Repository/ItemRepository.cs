using System.Linq.Expressions;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Framework;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Repository;

public class ItemRepository : IRepository<ItemModel>
{
    private readonly ApplicationDBContext _context;

    public ItemRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<ItemModel> GetByIdAsync(int itemId, Enum[] includes)
    {
        var query = GetQuery(includes);
        return await query.FirstOrDefaultAsync(i => i.ItemId == itemId);
    }

    public IQueryable<ItemModel> GetAll(Enum[] includes)
    {
        var query = GetQuery(includes);
        return query.AsNoTracking();
    }

    public IQueryable<ItemModel> Find(Expression<Func<ItemModel, bool>> where)
    {
        return _context.Item.AsNoTracking().Where(where);
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

    private IQueryable<ItemModel> GetQuery(Enum[] includes)
    {
        IQueryable<ItemModel> query = _context.Item;
        foreach (ItemIncludes include in includes)
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

        return query;
    }

    public async Task<ItemImageModel> GetItemImageByIdAsync(int itemImageId)
    {
        return await _context.ItemImage.FindAsync(itemImageId);
    }

    public async Task<IEnumerable<ItemModel>> FindAsync(
        Expression<Func<ItemModel, bool>> where,
        Enum[] includes
    )
    {
        var query = GetQuery(includes);
        return await query.AsNoTracking().Where(where).ToListAsync();
    }

    public async Task RemoveItemImageAsync(ItemImageModel itemImage)
    {
        _ = _context.ItemImage.Remove(itemImage);
        _ = await _context.SaveChangesAsync();
    }
}