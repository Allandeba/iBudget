using System.Linq.Expressions;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace iBudget.Repository;

public class CompanyRepository : IRepository<CompanyModel>
{
    private readonly ApplicationDBContext _context;

    public CompanyRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<CompanyModel> GetByIdAsync(int CompanyId, Enum[] includes)
    {
        var query = GetQuery(includes);
        return await query.FirstOrDefaultAsync(i => i.CompanyId == CompanyId);
    }

    public IQueryable<CompanyModel> GetAll(Enum[] includes)
    {
        var query = GetQuery(includes);
        return query.AsNoTracking();
    }

    public IQueryable<CompanyModel> Find(Expression<Func<CompanyModel, bool>> where)
    {
        return _context.Company.AsNoTracking().Where(where);
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

    private IQueryable<CompanyModel> GetQuery(Enum[] includes)
    {
        IQueryable<CompanyModel> query = _context.Company;
        foreach (CompanyIncludes include in includes)
            switch (include)
            {
                case CompanyIncludes.None:
                    break;

                default:
                    throw new Exception("CompanyIncludes type not implemented");
            }

        return query;
    }

    public async Task<IEnumerable<CompanyModel>> FindAsync(
        Expression<Func<CompanyModel, bool>> where,
        Enum[] includes
    )
    {
        var query = GetQuery(includes);
        return await query.AsNoTracking().Where(where).ToListAsync();
    }
}