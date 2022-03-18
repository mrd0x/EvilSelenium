using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Win32;
using System.Net;
namespace EvilSelenium.Commands
{
    public class InstallDependencies
    {
        // Depending on the version, a different chromedriver is installed.
        public static string checkChromeVersion()
        {
            const string EmptyChromeVersion = null;

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Google\\Chrome\\BLBeacon"))
                {
                    if (key != null)
                    {
                        Object val = key.GetValue("Version");
                        if (val != null)
                        {
                            Version version = new Version(val as String);
                            return val.ToString().Substring(0,2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return EmptyChromeVersion;
        }

        public static void InstallChromeDriver()
        {
            string chromeVersion = checkChromeVersion();
            const string chromeDriverName = "chromedriver.zip";
            Console.WriteLine("[+] Chrome version " + chromeVersion + " detected.");
            Console.WriteLine("[+] Downloading chromedriver...");

            var client = new WebClient();
            if (chromeVersion == "100")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/100.0.4896.20/chromedriver_win32.zip", chromeDriverName);
            }
            else if (chromeVersion == "99")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/99.0.4844.51/chromedriver_win32.zip", chromeDriverName);
            }
            else if (chromeVersion == "98")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/98.0.4758.48/chromedriver_win32.zip", chromeDriverName);
            }
            else if (chromeVersion == "97")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/97.0.4692.71/chromedriver_win32.zip", chromeDriverName);
            }
            else if(chromeVersion == "96")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/96.0.4664.45/chromedriver_win32.zip", chromeDriverName);
            }
            else if(chromeVersion == "95")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/95.0.4638.69/chromedriver_win32.zip", chromeDriverName);
            }
            else if (chromeVersion == "94")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/94.0.4606.113/chromedriver_win32.zip", chromeDriverName);
            }
            else if (chromeVersion == "93")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/93.0.4577.63/chromedriver_win32.zip", chromeDriverName);
            }
            else if (chromeVersion == "92")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/92.0.4515.107/chromedriver_win32.zip", chromeDriverName);
            }
            else if (chromeVersion == "91")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/91.0.4472.101/chromedriver_win32.zip", chromeDriverName);
            }
            else if(chromeVersion == "90")
            {
                client.DownloadFile("https://chromedriver.storage.googleapis.com/90.0.4430.24/chromedriver_win32.zip", chromeDriverName);
            }
            else
            {
                Console.WriteLine("[-] Couldn't install chromedriver.");
                return;
            }

            
            try
            {
                // Delete chromedriver.exe if it already exists
                File.Delete(@".\chromedriver.exe");
            }catch (Exception ex)
            {
                // Do nothing
            }

            // Unzipping
            Console.WriteLine("[+] Unzipping chromdriver.zip...");
            string zipPath = @".\chromedriver.zip";
            string extractPath = ".";
            ZipFile.ExtractToDirectory(zipPath, extractPath);
            Console.WriteLine("[+] Unzipping done.");
            Console.WriteLine("[+] Cleaning up...");
            File.Delete(@".\" + chromeDriverName);
            Console.WriteLine("[+] Cleaning done.");
            Console.WriteLine();

        }

        public static void InstallSeleniumDriver()
        {

            const string webdriverUnzippedPath = @".\tmp\lib\net47\WebDriver.dll";
            const string webdriverNamePath = @".\WebDriver.dll";
            const string seleniumZipPath = @".\selenium.zip";
            const string seleniumDownloadName = "selenium.zip";
            const string tmpFolder = @".\tmp";

            try
            {
                // Delete tmp folder if it already exists
                Directory.Delete(tmpFolder, true);
            }
            catch (Exception ex)
            {
                // Do nothing
            }
            
            
            // Install nuget package
            Console.WriteLine("[+] Installing Selenium WebDriver...");
            var client = new WebClient();
            client.DownloadFile("https://www.nuget.org/api/v2/package/Selenium.WebDriver/4.0.0", seleniumDownloadName);

            // Unzip
            ZipFile.ExtractToDirectory(seleniumZipPath, tmpFolder);

            try
            {
                // Copy driver to current folder
                File.Copy(webdriverUnzippedPath, webdriverNamePath);
            } catch(System.IO.IOException ex)
            {
                Console.WriteLine("[+] WebDriver already exists");
            }

            // Clean up
            Directory.Delete(tmpFolder, true);
            File.Delete(seleniumZipPath);

            Console.WriteLine("[+] Done.");

        }
    }
}
