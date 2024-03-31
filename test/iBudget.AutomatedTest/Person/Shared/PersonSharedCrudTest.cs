using OpenQA.Selenium;
using Shared;
using Shared.Extensions;

namespace iBudget.AutomatedTest.Person.Shared;

public class PersonSharedCrudTest : PersonSharedTest
{
    protected PersonSharedCrudTest() => _personMenu.Click();

    protected IWebElement _firstName => _driver.FindElement(By.Id("FirstName"));
    protected IWebElement _lastName => _driver.FindElement(By.Id("LastName"));
    protected IWebElement _email => _driver.FindElement(By.Id("Contact_Email"));
    protected IWebElement _phone => _driver.FindElement(By.Id("Contact_Phone"));
    protected IWebElement _documentType => _driver.FindElement(By.Id("Document_DocumentType"));
    protected IWebElement _document => _driver.FindElement(By.Id("Document_Document"));
    protected IWebElement? _lastAddedPerson => _driver.FindElements(By.Id("PersonItem")).FirstOrDefault();
}