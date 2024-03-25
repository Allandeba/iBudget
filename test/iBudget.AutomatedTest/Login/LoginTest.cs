using iBudget.AutomatedTest.Extensions;
using OpenQA.Selenium;
using Shared;

namespace iBudget.AutomatedTest.Login;

public class LoginTest : WebDriverFixture
{
    private IWebElement Username => _driver.FindElement(By.Id("Username"));
    private IWebElement Password => _driver.FindElement(By.Id("Password"));
    private string ExceptionMessage => _driver.FindElement(By.Id("exceptionMessageContent")).Text;
    
    [Fact]
    public void ShouldHaveDeveloperBadge()
    {
        Assert.NotNull(_driver.FindElement(By.Id("developer_badge")));
    }
    
    [Fact]
    public void ShouldBeAtLoginScreen()
    {
        Assert.True(_driver.Url.Contains("login", StringComparison.InvariantCultureIgnoreCase));
    }
    
    [Fact]
    public void ShouldHaveDefaultUserAndPassword()
    {
        Assert.Equal(Constants.DefaultUsername, Username.Value());
        Assert.Equal(Constants.DefaultPassword, Password.Value());
    }
    
    [Fact]
    public void ShouldDisplayMenuAfterSuccessfullyLogin()
    {
        Login();
        
        var menu = _driver.FindElement(By.Id("IndexMenu"));        
        Assert.NotNull(menu);
    }
    
    [Fact]
    public void ShouldHave5MenusDisplayedAfterLogin()
    {
        Login();
        
        var menu = _driver.FindElement(By.Id("IndexMenu"));
        Assert.Equal(5, menu.FindElements(By.TagName("object")).Count);
    }
    
    [Fact]
    public void ShouldFailUserLogin()
    {
        Username.SendKeys("outroUsuarioErro");
        Login();
        Assert.Equal(Messages.InvalidUsernameOrPassword, ExceptionMessage);
    }
    
    [Fact]
    public void ShouldFailPasswordLogin()
    {
        Password.SendKeys("senhaIncorreta");
        Login();
        Assert.Equal(Messages.InvalidUsernameOrPassword, ExceptionMessage);
    }
}