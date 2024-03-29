﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScraperFullStackMVC.Models;

namespace ScraperFullStackMVC.Controllers
{
    public class StocksController : Controller
    {
        private StockDatabaseEntities db = new StockDatabaseEntities();

        // GET: Stocks
        public ActionResult Index(string search)
        {
            if (search != null)
            {
                return View(db.Stocks.Where(x => x.symbol.StartsWith(search) || search == null).ToList());
            }
            else 
            {
                return View(db.Stocks.ToList());
            }
        }

        // SCRAPER CODE + SQL QUERIES
        public ActionResult ScrapeYahoo()
        {
            if (ModelState.IsValid)
            {
                DateTime myDateTime = DateTime.Now;

                var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StockDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();

                    var buttonScraper = new Scrape("gregsmith446@intracitygeeks.org", "SILICONrhode1!");

                    buttonScraper.LogIn();
                    buttonScraper.NavigateToPortfolio();
                    var snapshot = buttonScraper.ScrapeStockData();

                    foreach (var item in snapshot)
                    {
                        SqlCommand insCommand = new SqlCommand("INSERT INTO [Stocks] (symbol, price, change, pchange, currency, volume, marketcap, scrapetime) VALUES (@symbol, @price, @change, @pchange, @currency, @volume, @marketcap, @scrapetime)", sqlConnection);
                        insCommand.Parameters.AddWithValue("@symbol", item.Symbol.ToString());
                        insCommand.Parameters.AddWithValue("@price", item.Price.ToString());
                        insCommand.Parameters.AddWithValue("@change", item.Change.ToString());
                        insCommand.Parameters.AddWithValue("@pchange", item.PChange.ToString());
                        insCommand.Parameters.AddWithValue("@currency", item.Currency.ToString());
                        insCommand.Parameters.AddWithValue("@volume", item.Volume.ToString());
                        insCommand.Parameters.AddWithValue("@marketcap", item.MarketCap.ToString());
                        insCommand.Parameters.AddWithValue("@scrapetime", myDateTime);

                        insCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();

                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Stocks/Details/Id
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stocks stocks = db.Stocks.Find(id);
            if (stocks == null)
            {
                return HttpNotFound();
            }
            return View(stocks);
        }
    }
}
