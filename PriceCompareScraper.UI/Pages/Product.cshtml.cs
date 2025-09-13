using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PriceCompareScraper.Core.Bases;
using PriceCompareScraper.Core.Enums;
using PriceCompareScraper.Core.Models;
using PriceCompareScraper.Core.Services;
using System.Threading.Tasks;

namespace PriceCompareScraper.UI.Pages;

public class ProductModel : ModelBase<ProductModel>
{
    private readonly List<eProducts> _products;

    public ProductModel(ILogger<ProductModel> logger) : base(logger)
    {
        _products = Enum.GetValues(typeof(eProducts)).Cast<eProducts>().ToList();
    }

    private string[] Images;

    [BindProperty(SupportsGet = true)]
    public int Index { get; set; }

    //public int DbId { get; set; } // Index+1

    public string Image { get; private set; }
    public string Title { get; private set; }

    private List<SiteModel> _sites;
    public List<SiteModel> Sites
    {
        get
        {
            return _sites;
        }
        set
        {
            _sites = value;
        }
    }

    public async Task OnGet()
    {
        SetContext(HttpContext);
        Images = SetImages();
        Index = Indexer(Images, Index);

        Image = Images[Index];
        Title = GetTitleFromImage(Image);

        if (_cache.TryGet(eCacheKeys.Prices, out string pricesJson, eCacheType.All))
        {
            await SetSites(fromCache: true, pricesJson);
        }
        else
        {
            _ = Task.Run(async () =>
            {
                await SetSites(fromCache: false);
                var pricesJson = JsonConvert.SerializeObject(Sites);
                _cache.Set(eCacheKeys.Prices, pricesJson, eCacheType.All);

                // TODO: make the cookies storage update every hour

                // TODO: when the function is done, update the UI 
                // 1: SignalR, websockets
                // 2: Polling ()
            });
        }
    }

    private async Task SetSites(bool fromCache, string? pricesJson = null)
    {
        if (fromCache && !string.IsNullOrWhiteSpace(pricesJson))
        {
            Sites = JsonConvert.DeserializeObject<List<SiteModel>>(pricesJson);
        }
        else
        {
            Sites = new()
            {
                // TODO: Make Persistent SQL
                // TODO: Move to ModelBase
                new SiteModel(eSiteNames.Zap, "₪price", "https://www.zap.co.il/models.aspx?sog=e-SearchWord&orderby=price"),
                new SiteModel(eSiteNames.Payngo, "₪price", "https://www.payngo.co.il/instantsearchplus/result?q=SearchWord&sort=price_min_to_max"),
                new SiteModel(eSiteNames.Rozenfeld, "₪price", "https://www.rozenfeld.co.il/?s=SearchWord"),
                new SiteModel(eSiteNames.Alm, "₪price", "https://www.alm.co.il/search.html?query=SearchWord"),
                new SiteModel(eSiteNames.ShekemElectric, "₪price", "https://www.shekem-electric.co.il/instantsearchplus/result?q=SearchWord&sort=price_min_to_max")
            };

            for (int i = 0; i < Sites.Count; i++)
            {
                var site = Sites.ElementAt(i);
                var eProduct = _products.Find(p => (int)p == Index + 1);
                site = await Scrapper.SetSite(site, eProduct);
            }
        }
    }

    private string GetTitleFromImage(string imagePath)
    {
        var fileName = Path.GetFileName(imagePath);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var lastUnderscoreIndex = fileNameWithoutExtension.LastIndexOf('_');
        var title = lastUnderscoreIndex != -1 ? fileNameWithoutExtension.Substring(0, lastUnderscoreIndex) : fileNameWithoutExtension;

        //DbId = lastUnderscoreIndex != -1 ? int.Parse(fileNameWithoutExtension.Substring(lastUnderscoreIndex + 1)) : 0;

        return title.Replace("-", " ");
    }
}