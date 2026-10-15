using iBudget.AutomatedTest.Person.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shared;
using Shared.Extensions;
using Xunit.Extensions.Ordering;

namespace iBudget.AutomatedTest.Person.CrudHistory.Update;

[Order(2)]
public class PersonUpdateTest : PersonSharedCrudTest
{
    //TODO: Criar um ViewModel no Shared/ViewModels/PersonViewModel.cs e passar a separar a Entidade dos objetos de valores ViewModels.
    private const string PersonFirstNamePrefix = "Automated Updated";
    private const string PersonLastName = "Test Updated";
    private const string PersonEmail = "automated_updated@test.com";
    private readonly int PersonDocumentType = DocumentTypes.CNPJ.GetValue();

    private static string PersonFirstName(int personId) => $"{PersonFirstNamePrefix} {personId}";
    private static string PersonPhone(int personId) => $"+5549{personId:D9}";
    private static string PersonDocument(int personId) => $"22{personId:D12}";
    
    [Fact]
    public async Task ShouldUpdateLastAddedPerson()
    {
        Assert.NotNull(_lastAddedPerson);
        _lastAddedPerson!.Click();
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Contains(_personUpdateController, _uri.AbsolutePath);
        var personId = int.Parse(_uri.Segments[^1]);
        var originalCreationDate = _driver.FindElement(By.Id("CreationDate")).GetAttribute("value");

        _firstName.Clear();
        _firstName.SendKeys(PersonFirstName(personId));
        _lastName.Clear();
        _lastName.SendKeys(PersonLastName);
        _email.Clear();
        _email.SendKeys(PersonEmail);
        _phone.Clear();
        _phone.SendKeys(PersonPhone(personId));

        var selectElement = new SelectElement(_documentType);
        selectElement.SelectByValue(PersonDocumentType.ToString());
        _document.Clear();
        _document.SendKeys(PersonDocument(personId));

        _driver.FindElement(By.TagName("form")).Submit();
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_personController, _uri.AbsolutePath);

        Assert.NotNull(_lastAddedPerson);
        var personName = _lastAddedPerson!.FindElement(By.Id("PersonItemName"));
        Assert.Equal(PersonFirstName(personId) + ' ' + PersonLastName, personName.Text);

        _lastAddedPerson.Click();
        await Task.Delay(WaitTimeForUrlAssert);
        var persistedCreationDate = _driver.FindElement(By.Id("CreationDate")).GetAttribute("value");
        Assert.Equal(originalCreationDate, persistedCreationDate);
    }
    
    [Fact]
    public void ShouldFindUpdatedPerson()
    {
        Assert.NotNull(_lastAddedPerson);
        var personId = int.Parse(_lastAddedPerson.FindElement(By.TagName("button")).GetAttribute("id"));
        var personName = _lastAddedPerson.FindElement(By.Id("PersonItemName"));
        //TODO: no base criar um PersonModel (ou um ModelView e utilizar aqui a partir da classe)
        var expectedPersonName = PersonFirstName(personId) + ' ' + PersonLastName; 
        Assert.Equal(expectedPersonName, personName.Text);
    }
}
