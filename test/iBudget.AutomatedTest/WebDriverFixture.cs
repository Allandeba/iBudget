using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace iBudget.AutomatedTest;

public class WebDriverFixture : IDisposable
{
    protected readonly ChromeDriver _driver;

    public WebDriverFixture()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox");
        options.AddArguments("--disable-dev-shm-usage");
        options.AddArguments("--headless");
        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(1000);
        _driver.Manage().Window.Maximize();
        _driver.Navigate().GoToUrl("https://homologacao.ibudget.allandeba.dev.br/");
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