using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PriceCompareScraper.Core.Bases;
using PriceCompareScraper.Core.Enums;

namespace PriceCompareScraper.UI.Pages;

public class ProductModel : ModelBase<ProductModel>
{
    public ProductModel(ILogger<ProductModel> logger) : base(logger)
    {
        // ctor
    }

    private string[] Images;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public string Image { get; private set; }
    public string Title { get; private set; }

    public Dictionary<string, string> PriceSites => new()
    {
        { "Zap", "?1,999" },
        { "PriceCheck", "?1,950" },
        { "Geedo", "?2,050" },
        { "Google Shopping IL", "?1,980" },
        { "ShopX", "?1,970" }
    };

    public void OnGet()
    {
        SetImages();
        Id = Indexer();
        Image = Images[Id];
        Title = GetTitleFromImage(Image);
    }

    private void SetImages()
    {
        string imagesJson;
        if (_session.TryGet(eSessionKeys.Images, out imagesJson))
        {
            Images = JsonConvert.DeserializeObject<string[]>(imagesJson);
        }
        else
        {
            // Error if Product.cshtml is called without session containing eSessionKeys.Images
        }
    }

    private int Indexer()
    {
        return Id < 0
               ? 0
               : (Id >= Images.Length
               ? Images.Length - 1
               : Id);
    }

    private string GetTitleFromImage(string imagePath)
    {
        var fileName = Path.GetFileName(imagePath);
        return Path.GetFileNameWithoutExtension(fileName).Replace("-", " ");
    }
}