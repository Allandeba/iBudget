using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace iBudget.AutomatedTest;

public class WebDriverFixture : IDisposable
{
    private const string DevelopmentUrl = "http://127.0.0.1:5105";
    private const string StagingUrl = "https://hml-ibudget.allandeba.dev.br";
    private static readonly TimeSpan DefaultImplicitWait = TimeSpan.FromMilliseconds(1000);
    
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
        _driver.Manage().Timeouts().ImplicitWait = DefaultImplicitWait;

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

        var buttonLocator = IsDemoMode
            ? By.Id("demoLoginButton")
            : By.CssSelector("#formLogin button[type='submit']");

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        try
        {
            ClickWhenReady(buttonLocator);
            WaitForVisible(By.Id("IndexMenu"));
        }
        finally
        {
            _driver.Manage().Timeouts().ImplicitWait = DefaultImplicitWait;
        }
    }

    private void ClickWhenReady(By locator)
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        var button = wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(locator);
                if (!element.Displayed || !element.Enabled)
                    return null;

                var script = (IJavaScriptExecutor)driver;
                script.ExecuteScript(
                    "arguments[0].scrollIntoView({ block: 'center', inline: 'nearest' });",
                    element);

                var isUnobscured = (bool)script.ExecuteScript("""
                    const target = arguments[0];
                    const rect = target.getBoundingClientRect();
                    const hit = document.elementFromPoint(
                        rect.left + rect.width / 2,
                        rect.top + rect.height / 2);
                    return hit === target || target.contains(hit);
                    """, element);

                return isUnobscured ? element : null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        });

        button!.Click();
    }

    private void WaitForVisible(By locator)
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => driver.FindElements(locator).Any(element => element.Displayed));
    }
}
