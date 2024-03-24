using Bogus;
using iBudget.DAO.Entities;
using iBudget.Models.FakeModels.Helpers;

namespace iBudget.Models.FakeModels;

public static class ItemFakeModel
{
    public static async Task<List<ItemModel>> GetItemFakeModelList(int quantity)
    {
        var itemId = 0;

        var itemImageFaker = new Faker<ItemImageModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.Main, f => true)
            .RuleFor(c => c.FileName, f => f.Random.Guid().ToString())
            .RuleFor(c => c.ItemId, f => itemId++);

        var itemFaker = new Faker<ItemModel>(Constants.DefaultSystemLanguage)
            .RuleFor(c => c.ItemName, f => f.Lorem.Sentence(2))
            .RuleFor(c => c.Value, f => f.Random.Decimal(1, 100))
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(10, 3))
            .RuleFor(c => c.ItemImageList, f => itemImageFaker.Generate(1).ToList());

        var itemList = itemFaker.Generate(quantity);
        foreach (var item in itemList)
        foreach (var itemImage in item.ItemImageList)
            itemImage.ImageFile = await FakerHelper.GetRandomImage();

        return itemList;
    }
}