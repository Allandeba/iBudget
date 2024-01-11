using iBudget.Framework;
using iBudget.Framework.Helpers;
using iBudget.Models;
using iBudget.Repository;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Business
{
    public class PersonBusiness
    {
        private readonly PersonRepository _repository;

        public PersonBusiness(PersonRepository personRepository)
        {
            _repository = personRepository;
        }

        public async Task<IEnumerable<PersonModel>> GetPeople()
        {
            PersonIncludes[] includes = new PersonIncludes[] { PersonIncludes.None };
            IEnumerable<PersonModel> people = await _repository
                .GetAll(includes.Cast<Enum>().ToArray())
                .ToListAsync();
            return people.OrderByDescending(p => p.PersonId);
        }

        public async Task AddAsync(PersonModel person)
        {
            await _repository.AddAsync(person);
        }

        public async Task<PersonModel> GetByIdAsync(int personId)
        {
            PersonIncludes[] includes = new PersonIncludes[]
            {
                PersonIncludes.Contact,
                PersonIncludes.Document
            };
            return await _repository.GetByIdAsync(personId, includes.Cast<Enum>().ToArray());
        }

        public async Task UpdateAsync(PersonModel person)
        {
            PersonIncludes[] includes = new PersonIncludes[]
            {
                PersonIncludes.Contact,
                PersonIncludes.Document
            };
            PersonModel existentPerson = await _repository.GetByIdAsync(
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
            PersonIncludes[] includes = new PersonIncludes[]
            {
                PersonIncludes.Contact,
                PersonIncludes.Document
            };
            PersonModel person = await _repository.GetByIdAsync(
                personId,
                includes.Cast<Enum>().ToArray()
            );
            if (person == null)
                return;

            await _repository.RemoveAsync(person);
        }

        public async Task<IEnumerable<PersonModel>> GetAllLikeAsync(string search)
        {
            return string.IsNullOrEmpty(search)
                ? await GetPeople()
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
                    .ToListAsync();
        }
    }
}
