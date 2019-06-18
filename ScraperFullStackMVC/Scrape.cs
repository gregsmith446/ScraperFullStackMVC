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

        // blueprint for stock model/object
        public class Stock
        {
            public string Symbol { get; set; }
            public string Price { get; set; }
            public string Change { get; set; }
            public string PChange { get; set; }
            public string Volume { get; set; }
            public string MarketCap { get; set; }
            public string ScrapeTime { get; set; }
        }

        public List<Stock> Scrape()
        {
            ChromeOptions options = new ChromeOptions();

            // Add capbilities to ChromeOptions
            options.AddArguments("test -Type", "--ignore-certificate-errors", "--disable-gpu", "disable-popups");

            // Launching browser with desired capabilities + proper binary file location
            IWebDriver driver = new ChromeDriver(@"\Users\gregs\Desktop\CD\CapstoneConsoleApp\CapstoneConsoleApp\bin", options);
            driver.Manage().Window.Maximize();

            // create default wait 100 seconds + set to ignore the most persistent and disruptive timeout errors that break scraper
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100000));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException), typeof(UnhandledAlertException));

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

            driver.Navigate().GoToUrl("https://finance.yahoo.com/portfolio/p_0/view/v1");
            //EXPLICIT WAIT
            wait.Until(waiter => waiter.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[1]/td[13]/span")));

            IWebElement list = driver.FindElement(By.TagName("tbody"));
            ReadOnlyCollection<IWebElement> items = list.FindElements(By.TagName("tr"));
            int count = items.Count;

            List<Stock> stockList = new List<Stock>();

            for (int i = 1; i <= count; i++)
            {
                string symbol = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[1]/a")).GetAttribute("innerText");
                Console.WriteLine(symbol);
                string price = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[2]/span")).GetAttribute("innerText");
                Console.WriteLine(price);
                string change = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[3]/span")).GetAttribute("innerText");
                Console.WriteLine(change);
                string pchange = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[4]/span")).GetAttribute("innerText");
                Console.WriteLine(pchange);
                string volume = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[7]/span")).GetAttribute("innerText");
                Console.WriteLine(volume);
                string marketcap = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[13]/span")).GetAttribute("innerText");
                Console.WriteLine(marketcap);
                string scrapeTime = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table/tbody/tr[" + i + "]/td[6]/span")).GetAttribute("innerText");
                Console.WriteLine(scrapeTime);


                // for each stock entry, a new stock object is created
                Stock newStock = new Stock();
                newStock.Symbol = symbol;
                newStock.Price = price;
                newStock.Change = change;
                newStock.PChange = pchange;
                newStock.ScrapeTime = scrapeTime;
                newStock.Volume = volume;
                newStock.MarketCap = marketcap;

                // that stock is then added to the list of stocks
                stockList.Add(newStock);
            }

            driver.Quit();

            return stockList;
        }
    }
}

