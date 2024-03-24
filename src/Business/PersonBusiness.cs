using iBudget.DAO.Entities;
using iBudget.Framework;
using iBudget.Framework.Extensions;
using iBudget.Repository;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace iBudget.Business;

public class PersonBusiness
{
    private readonly PersonRepository _repository;

    public PersonBusiness(PersonRepository personRepository)
    {
        _repository = personRepository;
    }

    public async Task<IEnumerable<PersonModel>> GetPeople()
    {
        PersonIncludes[] includes = { PersonIncludes.None };
        IEnumerable<PersonModel> people = await _repository
            .GetAll(includes.Cast<Enum>().ToArray())
            .OrderByDescending(p => p.PersonId)
            .ToListAsync();
        return people;
    }

    public async Task<IPagedList<PersonModel>> GetPeoplePagination(
        int? pageNumber = Constants.InitialPageForPagination
    )
    {
        PersonIncludes[] includes = { PersonIncludes.None };
        var people = await _repository
            .GetAll(includes.Cast<Enum>().ToArray())
            .OrderByDescending(p => p.PersonId)
            .ToPagedListAsync(pageNumber, Constants.QtRegistersPagination);
        return people;
    }

    public async Task AddAsync(PersonModel person)
    {
        await _repository.AddAsync(person);
    }

    public async Task<PersonModel> GetByIdAsync(int personId)
    {
        PersonIncludes[] includes =
        {
            PersonIncludes.Contact,
            PersonIncludes.Document
        };
        return await _repository.GetByIdAsync(personId, includes.Cast<Enum>().ToArray());
    }

    public async Task UpdateAsync(PersonModel person)
    {
        PersonIncludes[] includes =
        {
            PersonIncludes.Contact,
            PersonIncludes.Document
        };
        var existentPerson = await _repository.GetByIdAsync(
            person.PersonId,
            includes.Cast<Enum>().ToArray()
        );

        if (existentPerson == null)
            return;

        existentPerson.Contact.Email = person.Contact.Email;
        existentPerson.Contact.Phone = person.Contact.Phone;

        existentPerson.Document.DocumentType = person.Document.DocumentType;
        existentPerson.Document.Document = person.Document.Document;

        existentPerson.FirstName = person.FirstName;
        existentPerson.LastName = person.LastName;

        await _repository.UpdateAsync(existentPerson);
    }

    public async Task RemoveAsync(int personId)
    {
        PersonIncludes[] includes =
        {
            PersonIncludes.Contact,
            PersonIncludes.Document
        };
        var person = await _repository.GetByIdAsync(
            personId,
            includes.Cast<Enum>().ToArray()
        );
        if (person == null)
            return;

        await _repository.RemoveAsync(person);
    }

    public async Task<IPagedList<PersonModel>> GetAllLikeAsync(
        string search,
        int? pageNumber = Constants.InitialPageForPagination
    )
    {
        return string.IsNullOrEmpty(search)
            ? await GetPeoplePagination(pageNumber)
            : await _repository
                .Find(
                    p =>
                        EF.Functions.ILike(
                            EF.Functions.Unaccent(p.FirstName),
                            $"%{search.Unaccent()}%"
                        )
                        || EF.Functions.ILike(
                            EF.Functions.Unaccent(p.LastName),
                            $"%{search.Unaccent()}%"
                        )
                )
                .ToPagedListAsync(pageNumber, Constants.QtRegistersPagination);
    }
}