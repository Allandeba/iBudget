using Bogus;
using Bogus.Extensions.Brazil;
using iBudget.Framework;

namespace iBudget.Models.FakeModels;

public static class PersonFakeModel
{
    public static List<PersonModel> GetPersonFakeModelList(int quantity)
    {
        var personDocumentId = 0;
        var personContactId = 0;

        var documentFaker = new Faker<DocumentModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.PersonId, f => personDocumentId++)
            .RuleFor(c => c.DocumentType, f => f.PickRandomParam<DocumentTypes>(DocumentTypes.CPF))
            .RuleFor(c => c.Document, f => f.Person.Cpf());

        var contactFaker = new Faker<ContactModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.PersonId, f => personContactId++)
            .RuleFor(c => c.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
            .RuleFor(c => c.Phone, f => f.Person.Phone);

        var personFaker = new Faker<PersonModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.LastName, f => f.Person.LastName)
            .RuleFor(c => c.Document, f => documentFaker.Generate())
            .RuleFor(c => c.Contact, f => contactFaker.Generate());

        return personFaker.Generate(quantity);
    }
}
