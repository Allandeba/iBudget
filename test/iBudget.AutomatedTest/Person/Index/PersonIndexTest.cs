using iBudget.AutomatedTest.Person.Shared;
using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Person.Index;

public class PersonIndexTest : PersonSharedCrudTest
{
    private IWebElement SearchBox => _driver.FindElement(By.Id("search-box"));
    private IWebElement PersonList => _driver.FindElement(By.Id("PersonList"));
    
    [Fact]
    public void ShouldHavePersonList()
    {
        Assert.NotNull(PersonList);
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
        Assert.Equal(_personSearchController, _uri.AbsolutePath);

        Assert.NotNull(PersonList);
        var persons = PersonList.FindElements(By.Id("PersonItem"));
        foreach (var person in persons)
        {
            var personName = person.FindElement(By.Id("PersonItemName")).Text;
            Assert.Contains("aB", personName, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    [Fact]
    public async Task ShouldSearchWithEnter()
    {
        Assert.NotNull(SearchBox);
        SearchBox.SendKeys("a");
        SearchBox.SendKeys(Keys.Enter);

        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_personSearchController, _uri.AbsolutePath);
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