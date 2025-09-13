using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using PriceCompareScraper.Core.Enums;
using PriceCompareScraper.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PriceCompareScraper.Core.Services
{
    public static class Scrapper
    {
        private static string GetSearchWord(SiteModel site, eProducts eProduct)
        {
            var searchWord = eProduct.ToString();
            if (site.Name == "Zap")
            {
                searchWord = GetEnumDescription(eProduct);
            }
            return searchWord;
        }
        private static string GetEnumDescription(eProducts eProduct)
        {
            var field = eProduct.GetType().GetField(eProduct.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                  .Cast<DescriptionAttribute>().FirstOrDefault();
            return attribute?.Description ?? eProduct.ToString();
        }

        public static async Task SetSite(SiteModel site, eProducts eProduct)
        {
            var searchWord = GetSearchWord(site, eProduct);
            site.BaseUrl = site.BaseUrl.Replace(eConstants.SearchWord.ToString(), searchWord);

            using var pw = await Playwright.CreateAsync();
            await using var browser = await pw.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = true });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync(site.BaseUrl);

            // find the price in this html
            if (site.BaseUrl != page.Url)
            {

            }

            site.FinalUrl = page.Url;
            site.Price = "";
        }
    }
}
