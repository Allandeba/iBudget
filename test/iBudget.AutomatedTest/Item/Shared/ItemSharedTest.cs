using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Item.Shared;

public class ItemSharedTest : WebDriverFixture
{
    protected ItemSharedTest() => Login();

    protected const string _itemController = "/Item";    
    protected const string _itemUpdateController = $"{_itemController}/Update";
    protected const string _itemCreateController = $"{_itemController}/Create";
    protected const string _itemSearchController = $"{_itemController}/Search";

    protected IWebElement _itemMenu => _driver.FindElement(By.Id("itemMenu")); 
}