using iBudget.DAO.Entities;
using iBudget.Framework;
using iBudget.Framework.Extensions;
using iBudget.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using X.PagedList;

namespace iBudget.Business;

public class ProposalBusiness
{
    private readonly CompanyBusiness _companyBusiness;
    private readonly ItemBusiness _itemBusiness;
    private readonly PersonBusiness _personBusiness;
    private readonly ProposalHistoryBusiness _proposalHistoryBusiness;
    private readonly ProposalRepository _repository;

    public ProposalBusiness(
        ProposalRepository ProposalRepository,
        PersonBusiness personBusiness,
        ItemBusiness itemBusiness,
        ProposalHistoryBusiness proposalHistoryBusiness,
        CompanyBusiness CompanyBusiness
    )
    {
        _repository = ProposalRepository;
        _personBusiness = personBusiness;
        _itemBusiness = itemBusiness;
        _proposalHistoryBusiness = proposalHistoryBusiness;
        _companyBusiness = CompanyBusiness;
    }

    public async Task<IEnumerable<ProposalModel>> GetProposals()
    {
        ProposalIncludes[] includes =
        {
            ProposalIncludes.Person,
            ProposalIncludes.ProposalHistory
        };
        IEnumerable<ProposalModel> proposals = await _repository
            .GetAll(includes.Cast<Enum>().ToArray())
            .OrderByDescending(p => p.ProposalId)
            .ToListAsync();
        return proposals;
    }

    public async Task<IPagedList<ProposalModel>> GetProposalsPagination(
        int? pageNumber = Constants.InitialPageForPagination
    )
    {
        ProposalIncludes[] includes =
        {
            ProposalIncludes.Person,
            ProposalIncludes.ProposalHistory
        };
        var proposals = await _repository
            .GetAll(includes.Cast<Enum>().ToArray())
            .OrderByDescending(p => p.ProposalId)
            .ToPagedListAsync(pageNumber, Constants.QtRegistersPagination);
        return proposals;
    }

    public async Task AddAsync(ProposalModel proposal)
    {
        proposal.Person = await GetPersonByIdAsync(proposal.PersonId);

        foreach (var proposalContent in proposal.ProposalContent)
            proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);

        await _repository.AddAsync(proposal);
    }

    public async Task<ProposalModel> GetByIdAsync(int proposalId)
    {
        ProposalIncludes[] includes =
        {
            ProposalIncludes.PersonContact,
            ProposalIncludes.Item
        };
        return await _repository.GetByIdAsync(proposalId, includes.Cast<Enum>().ToArray());
    }

    public async Task<ProposalModel> GetPrintByGUIDAsync(Guid GUID)
    {
        ProposalIncludes[] includes =
        {
            ProposalIncludes.PersonContact,
            ProposalIncludes.ItemImageList
        };
        return await _repository.GetByGUIDAsync(GUID, includes.Cast<Enum>().ToArray());
    }

    public async Task<ProposalModel> GetByGUIDAsync(Guid GUID)
    {
        ProposalIncludes[] includes =
        {
            ProposalIncludes.PersonContact,
            ProposalIncludes.Item
        };
        return await _repository.GetByGUIDAsync(GUID, includes.Cast<Enum>().ToArray());
    }

    public async Task<ProposalModel> GetPrintByIdAsync(int proposalId)
    {
        ProposalIncludes[] includes =
        {
            ProposalIncludes.Person,
            ProposalIncludes.ItemImageList
        };
        return await _repository.GetByIdAsync(proposalId, includes.Cast<Enum>().ToArray());
    }

    public async Task UpdateAsync(ProposalModel proposal)
    {
        foreach (var proposalContent in proposal.ProposalContent)
            proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);

        ProposalIncludes[] includes =
        {
            ProposalIncludes.Person,
            ProposalIncludes.ItemImageList
        };

        var existentProposal = await _repository.GetByIdAsync(
            proposal.ProposalId,
            includes.Cast<Enum>().ToArray()
        );

        if (existentProposal == null)
            throw new Exception(
                $"Não existe uma Proposal com o id == {proposal.ProposalId} para ser atualizada"
            );

        await CreateProposalHistoryAsync(existentProposal);

        await UpdateExistentProposalInformation(existentProposal, proposal);
        await _repository.UpdateAsync(existentProposal);
    }

    public async Task RemoveAsync(int proposalId)
    {
        ProposalIncludes[] includes =
        {
            ProposalIncludes.Person,
            ProposalIncludes.ItemImageList,
            ProposalIncludes.ProposalHistory
        };
        var proposal = await _repository.GetByIdAsync(
            proposalId,
            includes.Cast<Enum>().ToArray()
        );
        if (proposal == null)
            throw new Exception(
                $"Não foi encontrado uma Proposal com o id == {proposalId} para remoção"
            );

        await _repository.RemoveAsync(proposal);
    }

    private async Task<PersonModel> GetPersonByIdAsync(int personId)
    {
        return await _personBusiness.GetByIdAsync(personId);
    }

    private async Task CreateProposalHistoryAsync(ProposalModel existentProposal)
    {
        ProposalHistoryModel ProposalHistory =
            new()
            {
                ModificationDate = existentProposal.ModificationDate,
                PersonId = existentProposal.PersonId,
                ProposalId = existentProposal.ProposalId
            };

        ProposalContentJSON proposalContentJSON = new();
        foreach (var proposalContent in existentProposal.ProposalContent)
        {
            ProposalContentItems proposalContentItems =
                new()
                {
                    ItemId = proposalContent.ItemId.ToString(),
                    Quantity = proposalContent.Quantity.ToString()
                };
            proposalContentJSON.ProposalContentItems.Add(proposalContentItems);
        }

        ProposalHistory.ProposalContentJSON = proposalContentJSON;

        await _proposalHistoryBusiness.AddAsync(ProposalHistory);
    }

    private async Task UpdateExistentProposalInformation(
        ProposalModel existentProposal,
        ProposalModel proposalToUpdate
    )
    {
        existentProposal.PersonId = proposalToUpdate.PersonId;
        existentProposal.Discount = proposalToUpdate.Discount;

        await RemoveDeletedProposalContents(existentProposal, proposalToUpdate);

        // //Adiciona novos itens ou atualiza os existentes com seus novos valores
        foreach (var proposalContent in proposalToUpdate.ProposalContent)
        {
            var updateProposalContent = existentProposal.ProposalContent.Find(
                a => a.Item.ItemId == proposalContent.Item.ItemId
            );
            if (updateProposalContent == null)
            {
                existentProposal.ProposalContent.Add(proposalContent);
            }
            else
            {
                updateProposalContent.Item = proposalContent.Item;
                updateProposalContent.Quantity = proposalContent.Quantity;
            }
        }
    }

    private async Task RemoveDeletedProposalContents(
        ProposalModel existentProposal,
        ProposalModel proposalToUpdate
    )
    {
        var updatedProposalContentIds = proposalToUpdate.ProposalContent
            .Select(pc => pc.ProposalContentId)
            .ToList();

        var proposalContentToExclude =
            await _repository.FindProposalContentAsync(
                pc =>
                    !updatedProposalContentIds.Contains(pc.ProposalContentId)
                    && pc.ProposalId == proposalToUpdate.ProposalId
            );

        foreach (var exclude in proposalContentToExclude)
        {
            var item = existentProposal.ProposalContent.FirstOrDefault(
                x => x.ItemId == exclude.ItemId
            );
            if (item != null)
                existentProposal.ProposalContent.Remove(item);
            else
                throw new Exception("Não encontrou o item");
        }
    }

    public async Task<SelectList> GetSelectListPeople()
    {
        var people = await _personBusiness.GetPeople();
        return new SelectList(people, "PersonId", "PersonName");
    }

    public async Task<SelectList> GetSelectListItems()
    {
        var items = await _itemBusiness.GetItems();
        return new SelectList(items, "ItemId", "ItemName");
    }

    public async Task<IEnumerable<dynamic>> GetDynamicItems()
    {
        var items = await _itemBusiness.GetItems();
        return items
            .Select(
                i =>
                    new
                    {
                        i.ItemId,
                        i.ItemName,
                        i.Value
                    }
            )
            .ToList<dynamic>();
    }

    public async Task<CompanyModel> GetCompany()
    {
        return await _companyBusiness?.GetAllAsync();
    }

    public async Task<IPagedList<ProposalModel>> GetAllLikeAsync(
        string search,
        int? pageNumber = Constants.InitialPageForPagination
    )
    {
        if (search == null)
            return await GetProposalsPagination(pageNumber);

        ProposalIncludes[] includes = { ProposalIncludes.Person };
        var proposals = _repository.FindAsync(
            p =>
                EF.Functions.ILike(p.Person.FirstName, $"%{search.Unaccent()}%")
                || EF.Functions.ILike(p.Person.LastName, $"%{search.Unaccent()}%"),
            includes.Cast<Enum>().ToArray()
        );

        return await proposals
            .OrderByDescending(p => p.ProposalId)
            .ToPagedListAsync(pageNumber, Constants.QtRegistersPagination);
    }

    public async Task IncludePerson(ProposalModel proposal)
    {
        var person = await GetPersonByIdAsync(proposal.PersonId);
        proposal.Person = person;
    }

    public async Task IncludeItems(ProposalModel proposal)
    {
        if (proposal.ProposalContent.IsNullOrEmpty())
            return;

        foreach (var proposalContent in proposal.ProposalContent)
            proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);
    }
}