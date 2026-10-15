using iBudget.AutomatedTest.Item.Shared;
using OpenQA.Selenium;
using Xunit.Extensions.Ordering;

namespace iBudget.AutomatedTest.Item.CrudHistory.Update;

[Order(2)]
public class ItemUpdateTest : ItemSharedCrudTest
{
    //TODO: Criar um ViewModel no Shared/ViewModels/ItemViewModel.cs e passar a separar a Entidade dos objetos de valores ViewModels. 
    private const string ItemNamePrefix = "Automated item Updated";
    private const string ItemValue = "9.9";
    private const string ItemDescription = "automated description updated";

    private static string ItemName(int itemId) => $"{ItemNamePrefix} {itemId}";
    
    [Fact]
    public async Task ShouldUpdateLastAddedItem()
    {
        Assert.NotNull(_lastAddedItem);
        _lastAddedItem!.Click();
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Contains(_itemUpdateController, _uri.AbsolutePath);
        var itemId = int.Parse(_uri.Segments[^1]);

        _itemName.Clear();
        _itemName.SendKeys(ItemName(itemId));
        _itemValue.Clear();
        _itemValue.SendKeys(ItemValue);
        _itemDescription.Clear();
        _itemDescription.SendKeys(ItemDescription);

        //TODO: Trocar imagem padrão
        //TODO: Deletar imagem
        //TODO: Deletar imagem padrão
        
        _driver.FindElement(By.TagName("form")).Submit();
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_itemController, _uri.AbsolutePath);
    }
    
    [Fact]
    public void ShouldFindUpdatedItem()
    {
        Assert.NotNull(_lastAddedItem);
        var itemId = int.Parse(_lastAddedItem.FindElement(By.TagName("button")).GetAttribute("id"));
        var itemName = _lastAddedItem.FindElement(By.Id("itemName"));
        Assert.Equal(ItemName(itemId), itemName.Text);

        var itemDescription = _lastAddedItem.FindElement(By.Id("itemDescription"));
        Assert.Equal(ItemDescription, itemDescription.Text);
    }
}
