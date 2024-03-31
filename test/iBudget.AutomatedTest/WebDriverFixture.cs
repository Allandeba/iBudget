using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace iBudget.AutomatedTest;

public class WebDriverFixture : IDisposable
{
    protected readonly ChromeDriver _driver;
    protected string _exceptionMessage => _driver.FindElement(By.Id("exceptionMessageContent")).Text;
    protected Uri _uri => new Uri(_driver.Url); 
    
    protected WebDriverFixture()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox");
        options.AddArguments("--disable-dev-shm-usage");
        options.AddArguments("--headless");
        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(1000);
        _driver.Manage().Window.Maximize();
        //_driver.Navigate().GoToUrl("https://homologacao.ibudget.allandeba.dev.br");
        _driver.Navigate().GoToUrl("http://127.0.0.1:5105");
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