//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScraperFullStackMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        public int Id { get; set; }
        public string symbol { get; set; }
        public string price { get; set; }
        public string change { get; set; }
        public string pchange { get; set; }
        public string currency { get; set; }
        public string volume { get; set; }
        public string marketcap { get; set; }
        public System.DateTime scrapetime { get; set; }
    }
}
