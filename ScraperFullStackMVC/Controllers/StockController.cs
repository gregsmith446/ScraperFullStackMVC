using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScraperFullStackMVC.Models;
using static ScraperFullStackMVC.Scraper;

namespace ScraperFullStackMVC.Controllers
{
    public class StockController : Controller
    {
        private StockDatabaseEntities db = new StockDatabaseEntities();

        private static readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET: Stocks
        public ActionResult Index()
        {
            return View(db.Stock.ToList());
        }

        // route for logged in users to run the scraper
        [Authorize]
        public ActionResult Scrape()
        {
            Scraper myScraper = new Scraper();

            // Run scraper and save data to list stockList
            List<StockModel> stockList = myScraper.Scrape();

            // Open SQL connection
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            // Add new stock values to database
            foreach (StockModel stocks in stockList)
            {
                db.Stock.AddRange(stocks);
            }

            db.SaveChanges();

            // Must update times because GETDATE() is returning 1/1/0001 00:00:00
            SqlCommand updateTimes = new SqlCommand("UPDATE Stocks SET ScrapeTime = GETDATE()", conn);
            updateTimes.ExecuteNonQuery();

            conn.Close();
            return RedirectToAction("Index");
        }
    }
}
