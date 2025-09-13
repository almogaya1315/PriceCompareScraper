using PriceCompareScraper.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCompareScraper.Core.Models
{
    public class SiteModel
    {
        public SiteModel(eSiteNames name, string price, string baseUrl)
        {
            Name = name;
            Price = price;
            BaseUrl = baseUrl;
            FinalUrl = string.Empty;
        }

        public eSiteNames Name { get; set; }
        public string Price { get; set; }
        public string BaseUrl { get; set; }
        public string FinalUrl { get; set; }
    }
}
