using iBudget.DAO;
using iBudget.Framework;
using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace iBudget.Repository
{
    public class CompanyRepository : IRepository<CompanyModel>
    {
        private readonly ApplicationDBContext _context;

        public CompanyRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        private IQueryable<CompanyModel> GetQuery(Enum[] includes)
        {
            IQueryable<CompanyModel> query = _context.Company;
            foreach (CompanyIncludes include in includes)
            {
                switch (include)
                {
                    case CompanyIncludes.None:
                        break;

                    default:
                        throw new Exception("CompanyIncludes type not implemented");
                }
            }

            return query;
        }

        public async Task<CompanyModel> GetByIdAsync(int CompanyId, Enum[] includes)
        {
            IQueryable<CompanyModel> query = GetQuery(includes);
            return await query.FirstOrDefaultAsync(i => i.CompanyId == CompanyId);
        }

        public async Task<IEnumerable<CompanyModel>> GetAllAsync(Enum[] includes)
        {
            IQueryable<CompanyModel> query = GetQuery(includes);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<CompanyModel>> FindAsync(
            Expression<Func<CompanyModel, bool>> where
        )
        {
            return await _context.Company.Where(where).ToListAsync();
        }

        public async Task<IEnumerable<CompanyModel>> FindAsync(
            Expression<Func<CompanyModel, bool>> where,
            Enum[] includes
        )
        {
            IQueryable<CompanyModel> query = GetQuery(includes);
            return await query.Where(where).ToListAsync();
        }

        public async Task AddAsync(CompanyModel company)
        {
            _ = await _context.Company.AddAsync(company);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CompanyModel company)
        {
            _ = _context.Company.Update(company);
            _ = await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(CompanyModel company)
        {
            _ = _context.Company.Remove(company);
            _ = await _context.SaveChangesAsync();
        }
    }
}
