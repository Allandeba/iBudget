using iBudget.AutomatedTest.Person.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shared;
using Shared.Extensions;

namespace iBudget.AutomatedTest.Person.CrudHistory.Update;

public class PersonUpdateTest : PersonSharedCrudTest
{
    //TODO: Criar um ViewModel no Shared/ViewModels/PersonViewModel.cs e passar a separar a Entidade dos objetos de valores ViewModels.
    private const string PersonFirstName = "Automated Updated";
    private const string PersonLastName = "Test Updated";
    private const string PersonEmail = "automated_updated@test.com";
    private const string PersonPhone = "+5549000000000";
    private readonly int PersonDocumentType = DocumentTypes.CNPJ.GetValue();
    private const string PersonDocument = "11221122112211";
    
    [Fact]
    public void ShouldUpdateLastAddedPerson()
    {
        Assert.NotNull(_lastAddedPerson);
        _lastAddedPerson!.Click();
        Assert.Contains(_personUpdateController, _uri.AbsolutePath);

        _firstName.Clear();
        _firstName.SendKeys(PersonFirstName);
        _lastName.Clear();
        _lastName.SendKeys(PersonLastName);
        _email.Clear();
        _email.SendKeys(PersonEmail);
        _phone.Clear();
        _phone.SendKeys(PersonPhone);

        var selectElement = new SelectElement(_documentType);
        selectElement.SelectByValue(PersonDocumentType.ToString());
        _document.Clear();
        _document.SendKeys(PersonDocument);

        _driver.FindElement(By.TagName("form")).Submit();
        Assert.Equal(_personController, _uri.AbsolutePath);
    }
    
    [Fact]
    public void ShouldFindUpdatedPerson()
    {
        Assert.NotNull(_lastAddedPerson);
        
        var personName = _lastAddedPerson.FindElement(By.Id("PersonItemName"));
        //TODO: no base criar um PersonModel (ou um ModelView e utilizar aqui a partir da classe)
        var expectedPersonName = PersonFirstName + ' ' + PersonLastName; 
        Assert.Equal(expectedPersonName, personName.Text);

        var personCreationDate = _lastAddedPerson.FindElement(By.Id("PersonItemCreationDate"));
        Assert.Equal(DateTime.Now.ToString(Constants.ptBRDateFormat), personCreationDate.Text);
    }
}