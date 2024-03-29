﻿using iBudget.DAO;
using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Repository;

public class CatalogRepository
{
    private readonly ApplicationDBContext _context;

    public CatalogRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ItemModel>> GetItems()
    {
        return await _context.Item
            .AsNoTracking()
            .Include(i => i.ItemImageList)
            .OrderByDescending(a => a.ItemImageList.Count > 1)
            .ToListAsync();
    }
}