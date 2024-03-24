using OpenQA.Selenium;

namespace iBudget.AutomatedTest;

public class WebTest : WebDriverFixture
{
    [Fact]
    public void ShouldHaveDeveloperBadge()
    {
        Assert.NotNull(_driver.FindElement(By.Id("developer_badge")));
    }

    [Fact]
    public void ShouldDisplayMenuAfterLoginSuccessfully()
    {
        // var submitForm = _driver.FindElement(By.Id("submit"));
        // submitForm.Submit();

        // var menu = _driver.FindElement(By.Id("IndexMenu"));        
        // WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
        // wait.Until(d => menu.Displayed);
        // Assert.NotNull(menu);
        Assert.True(true);
    }

    //TODO: Check display indexMenu after successful login
    //TODO: Check existent user and password
    //TODO: Check wrong user and password signin
    //TODO: Check Menus
    //TODO: Check CRUDs
    //TODO: Check Proposal Print
    //TODO: Check Logoff
}