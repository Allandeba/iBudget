using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Person.Shared;

public class PersonSharedTest : WebDriverFixture
{
    protected PersonSharedTest() => Login();

    protected const string _personController = "/Person";    
    protected const string _personUpdateController = $"{_personController}/Update";
    protected const string _personCreateController = $"{_personController}/Create";
    protected const string _personSearchController = $"{_personController}/Search";

    protected IWebElement _personMenu => _driver.FindElement(By.Id("PersonMenu")); 
}