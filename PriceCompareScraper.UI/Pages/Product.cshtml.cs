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

    public List<SiteModel> Sites => new()
    {
        new SiteModel(eSiteNames.Zap, "₪price", "https://www.zap.co.il/models.aspx?sog=e-SearchWord&orderby=price"),
        //new SiteModel("Zap", "₪price", "https://www.zap.co.il/search.aspx?keyword=SearchWord&orderby=price"),
        new SiteModel(eSiteNames.Payngo, "₪price", "https://www.payngo.co.il/instantsearchplus/result?q=SearchWord&sort=price_min_to_max"),
        new SiteModel(eSiteNames.Rozenfeld, "₪price", "https://www.rozenfeld.co.il/?s=SearchWord"),
        new SiteModel(eSiteNames.Alm, "₪price", "https://www.alm.co.il/search.html?query=SearchWord"),
        new SiteModel(eSiteNames.ShekemElectric, "₪price", "https://www.shekem-electric.co.il/instantsearchplus/result?q=SearchWord&sort=price_min_to_max")
    };

    public async Task OnGet()
    {
        SetContext(HttpContext);
        Images = SetImages();
        Index = Indexer(Images, Index);

        Image = Images[Index];
        Title = GetTitleFromImage(Image);

        await SetSites();
    }

    private async Task SetSites()
    {
        for (int i = 0; i < Sites.Count; i++)
        {
            var site = Sites.ElementAt(i);
            var eProduct = _products.Find(p => (int)p == Index + 1);
            await Scrapper.SetSite(site, eProduct);
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