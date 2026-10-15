using iBudget.AutomatedTest.Extensions;
using OpenQA.Selenium;
using Shared;

namespace iBudget.AutomatedTest.Login;

public class LoginTest : WebDriverFixture
{
    private IWebElement Username => _driver.FindElement(By.Id("Username"));
    private IWebElement Password => _driver.FindElement(By.Id("Password"));

    [Fact]
    public void ShouldShowCurrentEnvironmentBadge()
    {
        Assert.NotNull(_driver.FindElement(By.Id(IsDemoMode ? "demo_badge" : "developer_badge")));
    }

    [Fact]
    public void ShouldBeAtLoginScreen()
    {
        Assert.True(_driver.Url.Contains("login", StringComparison.InvariantCultureIgnoreCase));
    }

    [Fact]
    public void ShouldOfferDemoAccessOrEmptyPassword()
    {
        if (IsDemoMode)
        {
            Assert.Empty(_driver.FindElements(By.Id("Username")));
            Assert.Empty(_driver.FindElements(By.Id("Password")));
            Assert.Empty(_driver.FindElements(By.Id("Remember")));
            Assert.Contains("Não precisa criar conta nem informar senha", _driver.FindElement(By.Id("demoDescription")).Text);
            Assert.Equal("Explorar demonstração", _driver.FindElement(By.Id("demoLoginButton")).Text);
        }
        else
        {
            Assert.Equal(Constants.DefaultUsername, Username.Value());
            Assert.Equal(string.Empty, Password.Value());
        }
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
    public void ShouldLogoff()
    {
        var wasDemo = IsDemoMode;
        Login();
        _driver.FindElement(By.Id("logoff")).Click();
        Assert.NotNull(_driver.FindElement(By.Id(wasDemo ? "formDemo" : "formLogin")));
        if (wasDemo)
            Assert.Empty(_driver.FindElements(By.Id("Password")));
        else
            Assert.NotNull(Password);
    }
}
