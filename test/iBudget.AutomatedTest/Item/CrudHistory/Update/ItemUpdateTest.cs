using iBudget.AutomatedTest.Item.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shared;
using Shared.Extensions;
using Xunit.Extensions.Ordering;

namespace iBudget.AutomatedTest.Item.CrudHistory.Update;

[Order(2)]
public class ItemUpdateTest : ItemSharedCrudTest
{
    //TODO: Criar um ViewModel no Shared/ViewModels/ItemViewModel.cs e passar a separar a Entidade dos objetos de valores ViewModels. 
    private const string ItemName = "Automated item Updated";
    private const string ItemValue = "9.9";
    private const string ItemDescription = "automated description updated";
    
    [Fact]
    public void ShouldUpdateLastAddedItem()
    {
        Assert.NotNull(_lastAddedItem);
        _lastAddedItem!.Click();
        Assert.Contains(_itemUpdateController, _uri.AbsolutePath);

        _itemName.Clear();
        _itemName.SendKeys(ItemName);
        _itemValue.Clear();
        _itemValue.SendKeys(ItemValue);
        _itemDescription.Clear();
        _itemDescription.SendKeys(ItemDescription);

        //TODO: Trocar imagem padrão
        //TODO: Deletar imagem
        //TODO: Deletar imagem padrão
        
        _driver.FindElement(By.TagName("form")).Submit();
        Assert.Equal(_itemController, _uri.AbsolutePath);
    }
    
    [Fact]
    public void ShouldFindUpdatedItem()
    {
        Assert.NotNull(_lastAddedItem);
        
        var itemName = _lastAddedItem.FindElement(By.Id("itemName"));
        Assert.Equal(ItemName, itemName.Text);

        var itemDescription = _lastAddedItem.FindElement(By.Id("itemDescription"));
        Assert.Equal(ItemDescription, itemDescription.Text);
    }
}