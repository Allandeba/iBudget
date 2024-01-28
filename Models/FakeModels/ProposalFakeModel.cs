using Bogus;
using iBudget.DAO.Entities;

namespace iBudget.Models.FakeModels;

public static class ProposalFakeModel
{
    public static List<ProposalModel> GetProposalFakeModelList(int quantity)
    {
        var proposalId = 0;

        var proposalContentFaker = new Faker<ProposalContentModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.ProposalId, f => proposalId++)
            .RuleFor(c => c.ItemId, f => f.Random.Int(1, 10))
            .RuleFor(c => c.Quantity, f => f.Random.Int(1, 10));

        var proposalFaker = new Faker<ProposalModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.ModificationDate, f => f.Date.Past())
            .RuleFor(c => c.Discount, f => f.Random.Decimal(0, 10))
            .RuleFor(c => c.GUID, f => f.Random.Guid())
            .RuleFor(c => c.PersonId, f => f.Random.Int(1, 10))
            .RuleFor(c => c.ProposalContent, f => proposalContentFaker.Generate(2).ToList());

        return proposalFaker.Generate(quantity);
    }
}
