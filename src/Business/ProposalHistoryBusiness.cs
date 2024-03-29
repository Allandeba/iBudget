﻿using iBudget.DAO.Entities;
using iBudget.Repository;
using Shared;

namespace iBudget.Business;

public class ProposalHistoryBusiness
{
    private readonly CompanyBusiness _companyBusiness;

    private readonly ItemBusiness _itemBusiness;
    private readonly ProposalHistoryRepository _repository;

    public ProposalHistoryBusiness(
        ProposalHistoryRepository ProposalHistoryRepository,
        ItemBusiness itemBusiness,
        CompanyBusiness CompanyBusiness
    )
    {
        _repository = ProposalHistoryRepository;
        _itemBusiness = itemBusiness;
        _companyBusiness = CompanyBusiness;
    }

    public async Task<ProposalModel> GetProposalFromHistory(int proposalHistoryId)
    {
        var proposalHistory = await GetByIdAsync(proposalHistoryId);

        ProposalModel proposal =
            new()
            {
                Person = proposalHistory.Person,
                ProposalContent = new List<ProposalContentModel>()
            };

        var items = await GetAllItemsAsync(proposalHistory);
        _ = proposalHistory.ProposalContentJSON.ProposalContentItems.OrderBy(p => p.ItemId);
        _ = items.OrderBy(i => i.ItemId);

        for (var i = 0; i < items.Count(); i++)
        {
            ProposalContentModel proposalContent =
                new()
                {
                    Proposal = proposal,
                    Item = items.ElementAt(i),
                    Quantity = int.Parse(
                        proposalHistory.ProposalContentJSON.ProposalContentItems[i].Quantity
                    )
                };

            proposal.ProposalContent.Add(proposalContent);
        }

        return proposal;
    }

    private async Task<IEnumerable<ItemModel>> GetAllItemsAsync(
        ProposalHistoryModel proposalHistory
    )
    {
        ItemIncludes[] includes = { ItemIncludes.ItemImage };

        var items = await _itemBusiness.FindAsync(
            i => proposalHistory.ProposalContentJSON.GetItemIds().Contains(i.ItemId),
            includes
        );
        return items;
    }

    public async Task AddAsync(ProposalHistoryModel proposalHistory)
    {
        await _repository.AddAsync(proposalHistory);
    }

    public async Task<ProposalHistoryModel> GetByIdAsync(int proposalHistoryId)
    {
        ProposalHistoryIncludes[] includes =
        {
            ProposalHistoryIncludes.Person,
            ProposalHistoryIncludes.Proposal
        };

        return await _repository.GetByIdAsync(
            proposalHistoryId,
            includes.Cast<Enum>().ToArray()
        );
    }

    public async Task<CompanyModel> GetCompany()
    {
        return await _companyBusiness!.GetAllAsync();
    }
}