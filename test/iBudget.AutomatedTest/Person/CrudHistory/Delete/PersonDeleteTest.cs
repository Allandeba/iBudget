using iBudget.AutomatedTest.Person.Shared;
using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Person.CrudHistory.Delete;

public class PersonDeleteTest : PersonSharedCrudTest
{
    [Fact]
    public void ShouldDeleteLastAddedPerson()
    {
        Assert.NotNull(_lastAddedPerson);
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