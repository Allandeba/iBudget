using iBudget.AutomatedTest.Item.Shared;
using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Item.Index;

public class ItemIndexTest : ItemSharedCrudTest
{
    private IWebElement SearchBox => _driver.FindElement(By.Id("search-box"));
    private IWebElement ItemList => _driver.FindElement(By.Id("itemList"));
    
    [Fact]
    public void ShouldHaveItemList()
    {
        Assert.NotNull(ItemList);
    }
    
    [Fact]
    public void ShouldHaveSearchField()
    {
        Assert.NotNull(SearchBox);
    }
    
    [Fact]
    public async Task ShouldSearchSuccessfully()
    {
        Assert.NotNull(SearchBox);
        SearchBox.SendKeys("aB");
        SearchBox.Submit();
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_itemSearchController, _uri.AbsolutePath);

        Assert.NotNull(ItemList);
        var items = ItemList.FindElements(By.Id("item"));
        foreach (var item in items)
        {
            var itemName = item.FindElement(By.Id("itemName")).Text;
            Assert.Contains("aB", itemName, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    [Fact]
    public async Task ShouldSearchWithEnter()
    {
        Assert.NotNull(SearchBox);
        SearchBox.SendKeys("a");
        SearchBox.SendKeys(Keys.Enter);
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_itemSearchController, _uri.AbsolutePath);
    }

    [Fact]
    public void ShouldNotBackPage()
    {
        var pagination = _driver.FindElement(By.ClassName("pagination"));
        Assert.NotNull(pagination);

        var skipToPrevisious = pagination.FindElement(By.ClassName("PagedList-skipToPrevious"));
        var classes = skipToPrevisious.GetAttribute("class");
        Assert.Contains("disabled", classes);
    }
}