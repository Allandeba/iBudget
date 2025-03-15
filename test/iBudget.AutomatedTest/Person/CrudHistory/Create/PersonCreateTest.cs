using iBudget.AutomatedTest.Extensions;
using iBudget.AutomatedTest.Person.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shared;
using Shared.Extensions;
using Xunit.Extensions.Ordering;

namespace iBudget.AutomatedTest.Person.CrudHistory.Create;

[Order(1)]
public class PersonCreateTest : PersonSharedCrudTest
{
    private IWebElement PersonCreate => _driver.FindElement(By.Id("PersonCreate"));
    
    //TODO: Criar um ViewModel no Shared/ViewModels/PersonViewModel.cs e passar a separar a Entidade dos objetos de valores ViewModels.
    private const string PersonFirstName = "Automated";
    private const string PersonLastName = "Test";
    private const string PersonEmail = "automated@test.com";
    private const string PersonPhone = "+5549999999999";
    private readonly int PersonDocumentType = DocumentTypes.CPF.GetValue();
    private const string PersonDocument = "11122233355";
    
    [Fact]
    public void DefaultValues()
    {
        PersonCreate.Click();
        
        Assert.Empty(_firstName.Text);
        Assert.Empty(_lastName.Text);
        
        Assert.Empty(_email.Text);
        Assert.NotEmpty(_email.GetAttribute("placeholder"));
        
        Assert.Empty(_phone.Text);
        Assert.NotEmpty(_phone.GetAttribute("placeholder"));

        Assert.Empty(_document.Text);
        var selectElement = new SelectElement(_documentType);
        Assert.False(selectElement.IsMultiple);
        Assert.Empty(selectElement.SelectedOption.Value());
    }

    [Fact]
    public async Task ShouldFindNewPerson()
    {
        await CreateNewPerson();
        Assert.NotNull(_lastAddedPerson);
        
        var personName = _lastAddedPerson.FindElement(By.Id("PersonItemName"));
        //TODO: no base criar um PersonModel (ou um ModelView e utilizar aqui a partir da classe)
        var expectedPersonName = PersonFirstName + ' ' + PersonLastName; 
        Assert.Equal(expectedPersonName, personName.Text);

        var personCreationDate = _lastAddedPerson.FindElement(By.Id("PersonItemCreationDate"));
        Assert.Equal(DateTime.Now.ToString(Constants.ptBRDateFormat), personCreationDate.Text);
    }

    private async Task CreateNewPerson()
    {
        PersonCreate.Click();
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_personCreateController, _uri.AbsolutePath);

        _firstName.SendKeys(PersonFirstName);
        _lastName.SendKeys(PersonLastName);
        _email.SendKeys(PersonEmail);
        _phone.SendKeys(PersonPhone);

        var selectElement = new SelectElement(_documentType);
        selectElement.SelectByValue(PersonDocumentType.ToString());
        _document.SendKeys(PersonDocument);

        _driver.FindElement(By.TagName("form")).Submit();
        await Task.Delay(WaitTimeForUrlAssert);
        Assert.Equal(_personController, _uri.AbsolutePath);
    }
}