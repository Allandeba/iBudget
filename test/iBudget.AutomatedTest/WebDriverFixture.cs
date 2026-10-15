using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace iBudget.AutomatedTest;

public class WebDriverFixture : IDisposable
{
    private const string DevelopmentUrl = "http://127.0.0.1:5105";
    private const string StagingUrl = "https://hml-ibudget.allandeba.dev.br";
    
    private bool IsDevelopment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    protected readonly ChromeDriver _driver;
    protected string _exceptionMessage => _driver.FindElement(By.Id("exceptionMessageContent")).Text;
    protected Uri _uri => new Uri(_driver.Url);
    protected const int WaitTimeForUrlAssert = 1500;
    protected bool IsDemoMode => _driver.FindElements(By.Id("demoLoginButton")).Count > 0;

    protected WebDriverFixture()
    {
        _ = DotNetEnv.Env.TraversePath().Load();

        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox");
        options.AddArguments("--disable-dev-shm-usage");
        options.AddArguments("--headless");
        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(1000);
        _driver.Manage().Window.Maximize();

        var urlRunner = Environment.GetEnvironmentVariable("IBUDGET_TEST_URL");
        if (string.IsNullOrWhiteSpace(urlRunner))
            urlRunner = IsDevelopment() ? DevelopmentUrl : StagingUrl;
        _driver.Navigate().GoToUrl(urlRunner);
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    protected void Login()
    {
        if (!IsDemoMode)
        {
            var password = Environment.GetEnvironmentVariable("USER_PASSWORD");
            if (string.IsNullOrEmpty(password))
                throw new InvalidOperationException("Configure USER_PASSWORD no executor Selenium para autenticar sem Demo.");

            var usernameInput = _driver.FindElement(By.Id("Username"));
            usernameInput.Clear();
            usernameInput.SendKeys(Shared.Constants.DefaultUsername);
            var passwordInput = _driver.FindElement(By.Id("Password"));
            passwordInput.Clear();
            passwordInput.SendKeys(password);
        }

        var button = IsDemoMode
            ? _driver.FindElement(By.Id("demoLoginButton"))
            : _driver.FindElement(By.CssSelector("#formLogin button[type='submit']"));
        button.Click();
    }
}
