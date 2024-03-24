using System.Linq.Expressions;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Framework;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Repository;

public class ProposalHistoryRepository : IRepository<ProposalHistoryModel>
{
    private readonly ApplicationDBContext _context;

    public ProposalHistoryRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public IQueryable<ProposalHistoryModel> GetAll(Enum[] includes)
    {
        var query = GetQuery(includes);
        return query.AsNoTracking();
    }

    public async Task<ProposalHistoryModel> GetByIdAsync(int proposalHistoryId, Enum[] includes)
    {
        var query = GetQuery(includes);
        return await query.FirstOrDefaultAsync(ph => ph.ProposalHistoryId == proposalHistoryId);
    }

    public IQueryable<ProposalHistoryModel> Find(
        Expression<Func<ProposalHistoryModel, bool>> where
    )
    {
        return _context.ProposalHistory.AsNoTracking().Where(where);
    }

    public async Task AddAsync(ProposalHistoryModel proposalHistory)
    {
        _ = await _context.ProposalHistory.AddAsync(proposalHistory);
        _ = await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProposalHistoryModel proposalHistory)
    {
        _ = _context.ProposalHistory.Update(proposalHistory);
        _ = await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(ProposalHistoryModel proposalHistory)
    {
        _ = _context.ProposalHistory.Remove(proposalHistory);
        _ = await _context.SaveChangesAsync();
    }

    private IQueryable<ProposalHistoryModel> GetQuery(Enum[] includes)
    {
        IQueryable<ProposalHistoryModel> query = _context.ProposalHistory;
        foreach (ProposalHistoryIncludes include in includes)
            switch (include)
            {
                case ProposalHistoryIncludes.None:
                    break;

                case ProposalHistoryIncludes.Person:
                    query = query.Include(p => p.Person);
                    break;

                case ProposalHistoryIncludes.Proposal:
                    query = query.Include(pp => pp.Proposal);
                    break;

                default:
                    throw new Exception("ProposalHistoryIncludes type not implemented");
            }

        return query;
    }
}