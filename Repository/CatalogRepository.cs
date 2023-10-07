﻿using getQuote.DAO;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Repository
{
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
                .Include(i => i.ItemImageList)
                .OrderByDescending(a => a.ItemImageList.Count > 1)
                .ToListAsync();
        }
    }
}
