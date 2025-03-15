using iBudget.AutomatedTest.Item.Shared;
using OpenQA.Selenium;
using Xunit.Extensions.Ordering;

namespace iBudget.AutomatedTest.Item.CrudHistory.Delete;

[Order(3)]
public class ItemDeleteTest : ItemSharedCrudTest
{
    [Fact]
    public async Task ShouldDeleteAutomatedLastAddedItem()
    {
        Assert.NotNull(_lastAddedItem);
        var itemName = _lastAddedItem.FindElement(By.Id("itemName"));
        Assert.Contains("Automated", itemName.Text);
        
        var deleteButton = _lastAddedItem.FindElement(By.TagName("button"));
        Assert.NotNull(deleteButton);
        var itemId = int.Parse(deleteButton.GetAttribute("id"));
        deleteButton.Click();
        
        var confirmationModal = _driver.FindElement(By.Id("confirmationModal"));
        Assert.NotNull(confirmationModal);

        var confirmationButton = confirmationModal.FindElement(By.Id("modalConfirmationButton"));
        Assert.NotNull(confirmationButton);
        confirmationButton.Click();
        
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_itemController, _uri.AbsolutePath);
    }
}