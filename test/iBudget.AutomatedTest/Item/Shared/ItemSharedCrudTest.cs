using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Item.Shared;

public class ItemSharedCrudTest : ItemSharedTest
{
    protected ItemSharedCrudTest() => _itemMenu.Click();

    protected IWebElement _itemName => _driver.FindElement(By.Id("ItemName"));
    protected IWebElement _itemValue => _driver.FindElement(By.Id("Value"));
    protected IWebElement _itemDescription => _driver.FindElement(By.Id("Description"));
    protected IWebElement _itemDefaultImageSelect => _driver.FindElement(By.Id("defaultImageSelect"));
    protected IWebElement _itemImageFiles => _driver.FindElement(By.Name("ImageFiles"));
    protected IWebElement? _lastAddedItem => _driver.FindElements(By.Id("item")).FirstOrDefault();
}