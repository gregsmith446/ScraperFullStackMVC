using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ScraperFullStackMVC.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace ScraperFullStackMVC
{
    public class Scraper
    {
        // build instance of scraper using below blueprints
        public Scraper()
        { }

        public List<StockModel> Scrape()
        {
            ChromeOptions options = new ChromeOptions();

            // Add capbilities to ChromeOptions
            options.AddArguments("test -Type", "--ignore-certificate-errors", "--disable-gpu", "disable-popups");
            // "--headless"

            // Launching browser with desired capabilities + proper binary file location
            IWebDriver driver = new ChromeDriver(@"\Users\gregs\Desktop\CD\CapstoneConsoleApp\CapstoneConsoleApp\bin", options);
            driver.Manage().Window.Maximize();

            // create default wait 10 seconds + set to ignore the most persistent and disruptive timeout errors that break scraper
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException), typeof(UnhandledAlertException), typeof(TimeoutException), typeof(WebDriverException));
            
            driver.Navigate().GoToUrl("https://login.yahoo.com/config/login?.src=finance&amp;.intl=us&amp;.done=https%3A%2F%2Ffinance.yahoo.com%2Fportfolios");

            wait.Until(waiter => waiter.FindElement(By.Id("login-username")));
            IWebElement username = driver.FindElement(By.Id("login-username"));
            username.SendKeys("gregsmith446@intracitygeeks.org");
            username.SendKeys(Keys.Return);

            // EXPLICIT WAIT
            wait.Until(waiter => waiter.FindElement(By.Id("login-passwd")));
            IWebElement password = driver.FindElement(By.Id("login-passwd"));
            password.SendKeys("SILICONrhode1!");
            IWebElement loginButton = driver.FindElement(By.Id("login-signin"));
            loginButton.SendKeys(Keys.Return);

            //EXPLICIT WAIT
            wait.Until(waiter => waiter.FindElement(By.XPath("//*[@id=\"uh-logo\"]")));

            driver.Navigate().GoToUrl("https://finance.yahoo.com/portfolio/p_0/view/v1");
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException), typeof(UnhandledAlertException), typeof(TimeoutException), typeof(WebDriverException));

            wait.Until(waiter => waiter.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[7]/td[1]/a")));
            IWebElement list = driver.FindElement(By.TagName("tbody"));
            ReadOnlyCollection<IWebElement> items = list.FindElements(By.TagName("tr"));
            int count = items.Count;

            List<StockModel> stockList = new List<StockModel>();

            for (int i = 1; i <= count; i++)
            {
                string symbol = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[1]/a")).GetAttribute("innerText");
                string price = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[2]/span")).GetAttribute("innerText");
                string change = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[3]/span")).GetAttribute("innerText");
                string pchange = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[4]/span")).GetAttribute("innerText");
                string currency = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[5]")).GetAttribute("innerText");
                string volume = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[7]/span")).GetAttribute("innerText");
                string marketcap = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[13]/span")).GetAttribute("innerText");

                // for each stock entry, a new stock object is created
                StockModel newStock = new StockModel
                {
                    Symbol = symbol,
                    Price = price,
                    Change = change,
                    PChange = pchange,
                    Currency = currency,
                    Volume = volume,
                    MarketCap = marketcap
                };

                // that stock is then added to the list of stocks
                stockList.Add(newStock);
            }

            driver.Quit();

            return stockList;
        }
    }
}

