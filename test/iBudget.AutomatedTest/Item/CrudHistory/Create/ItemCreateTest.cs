using iBudget.AutomatedTest.Extensions;
using iBudget.AutomatedTest.Item.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Extensions.Ordering;

namespace iBudget.AutomatedTest.Item.CrudHistory.Create;

[Order(1)]
public class ItemCreateTest : ItemSharedCrudTest
{
    private IWebElement ItemCreate => _driver.FindElement(By.Id("itemCreate"));
    
    //TODO: Criar um ViewModel no Shared/ViewModels/ItemViewModel.cs e passar a separar a Entidade dos objetos de valores ViewModels. 
    private const string ItemName = "Automated item";
    private const string ItemValue = "10,1";
    private const string ItemDescription = "automated description";
    
    [Fact]
    public void DefaultValues()
    {
        ItemCreate.Click();
        
        Assert.Empty(_itemName.Text);
        Assert.Equal("0,00", _itemValue.Value());
        Assert.Empty(_itemDescription.Text);
        
        var selectElement = new SelectElement(_itemDefaultImageSelect);
        Assert.False(selectElement.IsMultiple);
        Assert.Empty(selectElement.SelectedOption.Value());
    }

    [Fact]
    public void ShouldCreateNewItem()
    {
        ItemCreate.Click();
        Assert.Equal(_itemCreateController, _uri.AbsolutePath);

        _itemName.SendKeys(ItemName);
        _itemValue.Clear();
        _itemValue.SendKeys(ItemValue);
        _itemDescription.SendKeys(ItemDescription);
        
        //TODO: Fazer importar imagem e selecionar default
        
        _driver.FindElement(By.TagName("form")).Submit();
        Assert.Equal(_itemController, _uri.AbsolutePath);
    }

    [Fact]
    public void ShouldFindNewItem()
    {
        Assert.NotNull(_lastAddedItem);
        
        var itemName = _lastAddedItem.FindElement(By.Id("itemName"));
        Assert.Equal(ItemName, itemName.Text);

        var itemDescription = _lastAddedItem.FindElement(By.Id("itemDescription"));
        Assert.Equal(ItemDescription, itemDescription.Text);
    }
}