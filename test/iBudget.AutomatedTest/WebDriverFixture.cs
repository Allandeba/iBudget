using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace iBudget.AutomatedTest;

public class WebDriverFixture : IDisposable
{
    private const string DevelopmentUrl = "http://127.0.0.1:5105";
    private const string StagingUrl = "https://homologacao.ibudget.allandeba.dev.br";
    
    private bool IsDevelopment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    protected readonly ChromeDriver _driver;
    protected string _exceptionMessage => _driver.FindElement(By.Id("exceptionMessageContent")).Text;
    protected Uri _uri => new Uri(_driver.Url);

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

        var urlRunner = IsDevelopment() ? DevelopmentUrl : StagingUrl;
        _driver.Navigate().GoToUrl(urlRunner);
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    protected void Login()
    {
        var submitForm = _driver.FindElement(By.Id("submit"));
        submitForm.Submit();
    }
}