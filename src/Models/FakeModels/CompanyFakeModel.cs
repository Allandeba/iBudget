using Bogus;
using Bogus.Extensions.Brazil;
using iBudget.DAO.Entities;
using iBudget.Models.FakeModels.Helpers;

namespace iBudget.Models.FakeModels;

public static class CompanyFakeModel
{
    public static async Task<List<CompanyModel>> GetCompanyFakeModelList(int quantity)
    {
        var companyFaker = new Faker<CompanyModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.CompanyId, f => f.IndexFaker)
            .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
            .RuleFor(c => c.CNPJ, f => f.Company.Cnpj())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
            .RuleFor(c => c.Phone, f => f.Person.Phone);

        var companyList = companyFaker.Generate(quantity);

        foreach (var company in companyList)
            company.ImageFile = await FakerHelper.GetRandomImage();

        return companyList;
    }
}