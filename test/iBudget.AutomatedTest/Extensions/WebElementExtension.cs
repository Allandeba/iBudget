using OpenQA.Selenium;

namespace iBudget.AutomatedTest.Extensions;

public static class WebElementExtension
{
    public static string Value(this IWebElement content)
    {
        return content.GetAttribute("value");
    }
}