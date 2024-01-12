using iBudget.Framework;
using iBudget.Framework.Helpers;
using iBudget.Models;
using iBudget.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace iBudget.Business
{
    public class ProposalBusiness
    {
        private readonly ProposalRepository _repository;
        private readonly PersonBusiness _personBusiness;
        private readonly ItemBusiness _itemBusiness;
        private readonly ProposalHistoryBusiness _proposalHistoryBusiness;
        private readonly CompanyBusiness _companyBusiness;

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
            ProposalIncludes[] includes = new ProposalIncludes[]
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
            ProposalIncludes[] includes = new ProposalIncludes[]
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

            foreach (ProposalContentModel proposalContent in proposal.ProposalContent)
                proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);

            await _repository.AddAsync(proposal);
        }

        public async Task<ProposalModel> GetByIdAsync(int proposalId)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.PersonContact,
                ProposalIncludes.Item
            };
            return await _repository.GetByIdAsync(proposalId, includes.Cast<Enum>().ToArray());
        }

        public async Task<ProposalModel> GetPrintByGUIDAsync(Guid GUID)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.PersonContact,
                ProposalIncludes.ItemImageList
            };
            return await _repository.GetByGUIDAsync(GUID, includes.Cast<Enum>().ToArray());
        }

        public async Task<ProposalModel> GetByGUIDAsync(Guid GUID)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.PersonContact,
                ProposalIncludes.Item
            };
            return await _repository.GetByGUIDAsync(GUID, includes.Cast<Enum>().ToArray());
        }

        public async Task<ProposalModel> GetPrintByIdAsync(int proposalId)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.ItemImageList
            };
            return await _repository.GetByIdAsync(proposalId, includes.Cast<Enum>().ToArray());
        }

        public async Task UpdateAsync(ProposalModel proposal)
        {
            proposal.Person = await _personBusiness.GetByIdAsync(proposal.PersonId);

            foreach (ProposalContentModel proposalContent in proposal.ProposalContent)
                proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);

            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.ItemImageList
            };

            ProposalModel existentProposal = await _repository.GetByIdAsync(
                proposal.ProposalId,
                includes.Cast<Enum>().ToArray()
            );

            if (existentProposal == null)
                return;

            await CreateProposalHistoryAsync(existentProposal);

            await UpdateExistentProposalInformation(existentProposal, proposal);
            await _repository.UpdateAsync(existentProposal);
        }

        public async Task RemoveAsync(int proposalId)
        {
            ProposalIncludes[] includes = new ProposalIncludes[]
            {
                ProposalIncludes.Person,
                ProposalIncludes.ItemImageList
            };
            ProposalModel proposal = await _repository.GetByIdAsync(
                proposalId,
                includes.Cast<Enum>().ToArray()
            );
            if (proposal == null)
                return;

            foreach (ProposalContentModel proposalContent in proposal.ProposalContent)
            {
                ProposalContentModel dBProposalContent =
                    await _repository.GetProposalContentByIdAsync(
                        proposalContent.ProposalContentId
                    );
                await _repository.RemoveProposalContentAsync(dBProposalContent);
            }

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
                    Person = existentProposal.Person,
                    Proposal = existentProposal
                };

            ProposalContentJSON proposalContentJSON = new();
            foreach (ProposalContentModel proposalContent in existentProposal.ProposalContent)
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
            existentProposal.Person = proposalToUpdate.Person;
            existentProposal.Discount = proposalToUpdate.Discount;

            await RemoveDeletedProposalContents(existentProposal, proposalToUpdate);

            //Adiciona novos itens ou atualiza os existentes com seus novos valores
            foreach (ProposalContentModel proposalContent in proposalToUpdate.ProposalContent)
            {
                ProposalContentModel updateProposalContent = existentProposal.ProposalContent.Find(
                    a => a.Item.ItemId == proposalContent.Item.ItemId
                );
                if (updateProposalContent == null)
                    existentProposal.ProposalContent.Add(proposalContent);
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
            // ProposalContent enviados da view
            List<int> updatedProposalContentIds = proposalToUpdate.ProposalContent
                .Select(pc => pc.ProposalContentId)
                .ToList();

            // Pega todos os ProposalContent que estão no banco mas foram excluidos na view
            IEnumerable<ProposalContentModel> proposalContentToExclude =
                await _repository.FindProposalContentAsync(
                    pc =>
                        !updatedProposalContentIds.Contains(pc.ProposalContentId)
                        && pc.ProposalId == proposalToUpdate.ProposalId
                );
            foreach (ProposalContentModel existentProposalContent in proposalContentToExclude)
                _ = existentProposal.ProposalContent.Remove(existentProposalContent);
        }

        public async Task<SelectList> GetSelectListPeople()
        {
            IEnumerable<PersonModel> people = await _personBusiness.GetPeople();
            return new SelectList(people, "PersonId", "PersonName");
        }

        public async Task<SelectList> GetSelectListItems()
        {
            IEnumerable<ItemModel> items = await _itemBusiness.GetItems();
            return new SelectList(items, "ItemId", "ItemName");
        }

        public async Task<IEnumerable<dynamic>> GetDynamicItems()
        {
            IEnumerable<ItemModel> items = await _itemBusiness.GetItems();
            return items
                .Select(
                    i =>
                        new
                        {
                            i.ItemId,
                            i.ItemName,
                            i.Value,
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

            ProposalIncludes[] includes = new ProposalIncludes[] { ProposalIncludes.Person };
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
            PersonModel person = await GetPersonByIdAsync(proposal.PersonId);
            proposal.Person = person;
        }

        public async Task IncludeItems(ProposalModel proposal)
        {
            foreach (ProposalContentModel proposalContent in proposal.ProposalContent)
                proposalContent.Item = await _itemBusiness.GetByIdAsync(proposalContent.ItemId);
        }
    }
}
