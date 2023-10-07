﻿using getQuote.DAO;
using getQuote.Framework;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace getQuote.Repository
{
    public class ProposalRepository : IRepository<ProposalModel>
    {
        private readonly ApplicationDBContext _context;

        public ProposalRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        private IQueryable<ProposalModel> GetQuery(Enum[] includes)
        {
            IQueryable<ProposalModel> query = _context.Proposal;
            foreach (ProposalIncludes include in includes)
            {
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
            }

            return query;
        }

        public async Task<IEnumerable<ProposalModel>> GetAllAsync(Enum[] includes)
        {
            IQueryable<ProposalModel> query = GetQuery(includes);
            return await query.ToListAsync();
        }

        public async Task<ProposalModel> GetByIdAsync(int proposalId, Enum[] includes)
        {
            IQueryable<ProposalModel> query = GetQuery(includes);
            return await query.FirstOrDefaultAsync(p => p.ProposalId == proposalId);
        }

        public async Task<ProposalModel> GetByGUIDAsync(Guid GUID, Enum[] includes)
        {
            IQueryable<ProposalModel> query = GetQuery(includes);
            return await query.FirstOrDefaultAsync(p => p.GUID.Equals(GUID));
        }

        public async Task<ProposalContentModel> GetProposalContentByIdAsync(int proposalContentId)
        {
            return await _context.ProposalContent.FirstOrDefaultAsync(
                pc => pc.ProposalContentId == proposalContentId
            );
        }

        public async Task<IEnumerable<ProposalModel>> FindAsync(
            Expression<Func<ProposalModel, bool>> where
        )
        {
            return await _context.Proposal.Where(where).ToListAsync();
        }

        public async Task<IEnumerable<ProposalModel>> FindAsync(
            Expression<Func<ProposalModel, bool>> where,
            Enum[] includes
        )
        {
            IQueryable<ProposalModel> query = GetQuery(includes);
            return await query.Where(where).ToListAsync();
        }

        public async Task<IEnumerable<ProposalContentModel>> FindProposalContentAsync(
            Expression<Func<ProposalContentModel, bool>> where
        )
        {
            return await _context.ProposalContent.Where(where).ToListAsync();
        }

        public async Task AddAsync(ProposalModel Proposal)
        {
            _ = await _context.Proposal.AddAsync(Proposal);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProposalModel Proposal)
        {
            _ = _context.Proposal.Update(Proposal);
            _ = await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ProposalModel Proposal)
        {
            _ = _context.Proposal.Remove(Proposal);
            _ = await _context.SaveChangesAsync();
        }

        public async Task RemoveProposalContentAsync(ProposalContentModel proposalContent)
        {
            _ = _context.ProposalContent.Remove(proposalContent);
            _ = await _context.SaveChangesAsync();
        }
    }
}
