using OpenQA.Selenium.Chrome;

namespace iBudget.AutomatedTest
{
    public class WebDriverFixture : IDisposable
    {
        protected readonly ChromeDriver _driver;
        public WebDriverFixture()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(1000);
            _driver.Manage().Window.Maximize(); 
            _driver.Navigate().GoToUrl("https://ibudget.allandeba.dev.br");
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}