using iBudget.AutomatedTest.Person.Shared;
using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Person.Index;

public class PersonIndexTest : PersonSharedCrudTest
{
    [Fact]
    public void ShouldHavePersonList()
    {
        var personList = _driver.FindElement(By.Id("PersonList"));
        Assert.NotNull(personList);
    }
    
    [Fact]
    public void ShouldHaveSearchField()
    {
        var searchField = _driver.FindElement(By.Id("search-box"));
        Assert.NotNull(searchField);
    }
}