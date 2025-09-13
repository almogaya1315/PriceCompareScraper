using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PriceCompareScraper.Core.Enums;
using PriceCompareScraper.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PriceCompareScraper.Core.Bases
{
    public abstract class ModelBase<T> : PageModel
    {
        protected readonly ILogger<T> _logger;
        protected CacheHandler _cache;

        public ModelBase(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected void SetContext(HttpContext context)
        {
            _cache = new CacheHandler(context);
        }

        protected string[] SetImages(bool init = false)
        {
            string[] images = [];
            if (_cache.TryGet(eCacheKeys.Images, out string imagesJson, eCacheType.Session))
            {
                images = JsonConvert.DeserializeObject<string[]>(imagesJson);
            }
            else if (init)
            {
                images = SetImages();
            }
            else
            {
                // Error if Product.cshtml is called without session containing eCacheKeys.Images
            }
            return images;
        }

        private string[] SetImages()
        {
            string[] images =
                [
                    // TODO: Make Persistent SQL
                    "Images\\Dishwasher_1.jpg",
                    "Images\\Microwave_2.jpg",
                    "Images\\Ninja_3.jpg",
                    "Images\\Oven_4.jpg",
                    "Images\\Refrigirator_5.jpg",
                ];
            var imagesJson = JsonConvert.SerializeObject(images);
            _cache.Set(eCacheKeys.Images, imagesJson, eCacheType.Session);
            return images;
        }

        protected int Indexer(string[] arr, int index)
        {
            return index < 0                            // if index below first
                         ? 0                            // limit to first (0)
                         : (index >= arr.Length         // else, if index beyond last
                         ? arr.Length - 1               // limit to last (Length-1)
                         : index);                      // else, keep index as-is
        }
    }
}
