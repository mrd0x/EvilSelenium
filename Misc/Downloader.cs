using System;
using OpenQA.Selenium;
using EvilSelenium.Misc;
using System.Threading;

namespace EvilSelenium.Misc
{
    class Downloader
    {
        public static void DownloadFile(string fileURL, string seconds)
        {
            int ms = 0;

            try
            {
                // Convert seconds to milliseconds
                int secondsCasted = Int32.Parse(seconds);
                ms = secondsCasted * 1000;
            }catch(FormatException ex)
            {
                Console.WriteLine("[-] Error - incorrect time format specified");
                System.Environment.Exit(-1);
            }

            IWebDriver driver = Helpers.InitDriver();
            try
            {
                driver.Navigate().GoToUrl(fileURL);
            } catch (WebDriverArgumentException ex)
            {
                Console.WriteLine("[-] Invalid URL");
                System.Environment.Exit(-1);
            }
            Thread.Sleep(ms);

            driver.Close();
            driver.Quit();

        }
    }
}
