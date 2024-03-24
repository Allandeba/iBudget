using System.Linq.Expressions;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Framework;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Repository;

public class ProposalRepository : IRepository<ProposalModel>
{
    private readonly ApplicationDBContext _context;

    public ProposalRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public IQueryable<ProposalModel> GetAll(Enum[] includes)
    {
        var query = GetQuery(includes);
        return query.AsNoTracking();
    }

    public async Task<ProposalModel> GetByIdAsync(int proposalId, Enum[] includes)
    {
        var query = GetQuery(includes);
        return await query.FirstOrDefaultAsync(p => p.ProposalId == proposalId);
    }

    public IQueryable<ProposalModel> Find(Expression<Func<ProposalModel, bool>> where)
    {
        return _context.Proposal.AsNoTracking().Where(where);
    }

    public async Task AddAsync(ProposalModel Proposal)
    {
        await _context.Proposal.AddAsync(Proposal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProposalModel Proposal)
    {
        _context.Proposal.Update(Proposal);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(ProposalModel Proposal)
    {
        _context.Proposal.Remove(Proposal);
        await _context.SaveChangesAsync();
    }

    private IQueryable<ProposalModel> GetQuery(Enum[] includes)
    {
        IQueryable<ProposalModel> query = _context.Proposal;
        foreach (ProposalIncludes include in includes)
            switch (include)
            {
                case ProposalIncludes.None:
                    break;

                case ProposalIncludes.Person:
                    query = query.Include(p => p.Person);
                    break;

                case ProposalIncludes.PersonContact:
                    query = query.Include(p => p.Person).ThenInclude(p => p.Contact);
                    break;

                case ProposalIncludes.ProposalHistory:
                    query = query.Include(ph => ph.ProposalHistory);
                    break;

                case ProposalIncludes.Item:
                    query = query.Include(pc => pc.ProposalContent).ThenInclude(pc => pc.Item);
                    break;

                case ProposalIncludes.ItemImageList:
                    query = query
                        .Include(pc => pc.ProposalContent)
                        .ThenInclude(pc => pc.Item)
                        .ThenInclude(ii => ii.ItemImageList);
                    break;

                default:
                    throw new Exception("ProposalIncludes type not implemented");
            }

        return query;
    }

    public async Task<ProposalModel> GetByGUIDAsync(Guid GUID, Enum[] includes)
    {
        var query = GetQuery(includes);
        return await query.FirstOrDefaultAsync(p => p.GUID.Equals(GUID));
    }

    public async Task<ProposalContentModel> GetProposalContentByIdAsync(int proposalContentId)
    {
        return await _context.ProposalContent.FirstOrDefaultAsync(
            pc => pc.ProposalContentId == proposalContentId
        );
    }

    public IQueryable<ProposalModel> FindAsync(
        Expression<Func<ProposalModel, bool>> where,
        Enum[] includes
    )
    {
        var query = GetQuery(includes);
        return query.AsNoTracking().Where(where);
    }

    public async Task<IEnumerable<ProposalContentModel>> FindProposalContentAsync(
        Expression<Func<ProposalContentModel, bool>> where
    )
    {
        return await _context.ProposalContent.AsNoTracking().Where(where).ToListAsync();
    }

    public async Task RemoveProposalContentAsync(ProposalContentModel proposalContent)
    {
        _context.ProposalContent.Remove(proposalContent);
        await _context.SaveChangesAsync();
    }
}