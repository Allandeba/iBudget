using iBudget.AutomatedTest.Person.Shared;
using OpenQA.Selenium;
using Xunit.Extensions.Ordering;

namespace iBudget.AutomatedTest.Person.CrudHistory.Delete;

[Order(3)]
public class PersonDeleteTest : PersonSharedCrudTest
{
    [Fact]
    public void ShouldDeleteAutomatedLastAddedPerson()
    {
        Assert.NotNull(_lastAddedPerson);
        var personName = _lastAddedPerson.FindElement(By.Id("PersonItemName"));
        Assert.Contains("Automated", personName.Text);
        
        var deleteButton = _lastAddedPerson.FindElement(By.TagName("button"));
        Assert.NotNull(deleteButton);
        var personId = int.Parse(deleteButton.GetAttribute("id"));
        deleteButton.Click();
        
        var confirmationModal = _driver.FindElement(By.Id("confirmationModal"));
        Assert.NotNull(confirmationModal);

        var confirmationButton = confirmationModal.FindElement(By.Id("modalConfirmationButton"));
        Assert.NotNull(confirmationButton);
        confirmationButton.Click();
        
        Assert.Equal(_personController, _uri.AbsolutePath);
    }
}