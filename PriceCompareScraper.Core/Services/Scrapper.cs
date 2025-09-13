using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using PriceCompareScraper.Core.Enums;
using PriceCompareScraper.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
            if (site.Name == eSiteNames.Zap)
            {
                //searchWord = GetEnumDescription(eProduct);
                if (eProduct == eProducts.Microwave)
                    searchWord = eProduct.ToString() + "Oven"; 
                if (eProduct == eProducts.Ninja)
                    searchWord = "foodproccessor&db136390=10840000";
            }
            return searchWord;
        }
        //private static string GetEnumDescription(eProducts eProduct)
        //{
        //    var field = eProduct.GetType().GetField(eProduct.ToString());
        //    var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
        //                          .Cast<DescriptionAttribute>().FirstOrDefault();
        //    return attribute?.Description ?? eProduct.ToString();
        //}

        private static string BaseUrl(SiteModel site)
        {
            var baseUri = new Uri(site.BaseUrl);
            return $"{baseUri.Scheme}://{baseUri.Host}/";
        }

        public static async Task SetSite(SiteModel site, eProducts eProduct)
        {
            var searchWord = GetSearchWord(site, eProduct);
            site.BaseUrl = site.BaseUrl.Replace(eConstants.SearchWord.ToString(), searchWord);

            using var pw = await Playwright.CreateAsync();
            await using var browser = await pw.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = false });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync(site.BaseUrl);

            //Zap
            var toProduct = page.Locator(".zap-btn-compare:not(.to-compare-list-btn)");
            //ninja
            if (await toProduct.CountAsync() == 0)
            {
                toProduct = page.Locator(".OptionClicks3.zap-btn").First;
                site.FinalUrl = BaseUrl(site) + await toProduct.GetAttributeAsync("href");
                var price = page.Locator(".price-wrapper.product.total").InnerTextAsync().Result.Replace("₪", "");
            }
            //dishwasher
            //microwave
            else
            {
                await toProduct.First.ClickAsync();
                site.FinalUrl = page.Url;
                var price = page.Locator(".price-value.total").InnerTextAsync().Result.Replace("₪", "");
                site.Price = site.Price.Remove(1) + price;
            }
                
            //oven
            //fridge

            //Payngo
            //dishwasher
            //microwave
            //ninja
            //oven
            //fridge

            //Rozenfeld
            //dishwasher
            //microwave
            //ninja
            //oven
            //fridge

            //Alm
            //dishwasher
            //microwave
            //ninja
            //oven
            //fridge

            //ShekemElectric
            //dishwasher
            //microwave
            //ninja
            //oven
            //fridge
        }
    }
}
