using System;
using EvilSelenium.Misc;
using OpenQA.Selenium;

namespace EvilSelenium.Cookies
{
    class ExtractCookies
    {
        public static void PrintCookies(string website)
        {
            IWebDriver driver = Helpers.InitDriver();

            try
            {
                driver.Navigate().GoToUrl(website);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
            } catch(WebDriverArgumentException ex)
            {
                Console.WriteLine("[-] Invalid URL");
            }
            var _cookies = driver.Manage().Cookies.AllCookies;
            foreach (var cookie in _cookies)
            {
                Console.WriteLine(cookie);
            }

            driver.Quit();
        }
    }
}
