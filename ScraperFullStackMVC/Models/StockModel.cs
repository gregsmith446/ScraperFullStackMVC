using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraperFullStackMVC.Models
{
    public partial class StockModel
    {
        public string Symbol { get; set; }
        public string Price { get; set; }
        public string Change { get; set; }
        public string PChange { get; set; }
        public string Currency { get; set; }
        public string Volume { get; set; }
        public string MarketCap { get; set; }
        public System.DateTime ScrapeTime { get; set; }
    }
}