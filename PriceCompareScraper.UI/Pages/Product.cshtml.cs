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
    public int Index { get; set; }

    //public int DbId { get; set; } // Index+1

    public string Image { get; private set; }
    public string Title { get; private set; }

    public List<(string Name, string Price, string Url)> PriceSites => new()
    {
        ("Zap", "₪1,999", ""),
        ("PriceCheck", "₪1,950", ""),
        ("Geedo", "₪2,050", ""),
        ("Google Shopping IL", "₪1,980", ""),
        ("ShopX", "₪1,970", "")
    };

    public void OnGet()
    {
        SetContext(HttpContext);
        Images = SetImages();
        Index = Indexer(Images, Index);

        Image = Images[Index];
        Title = GetTitleFromImage(Image);
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