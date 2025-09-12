using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PriceCompareScraper.UI.Pages;

public class ProductModel : PageModel
{
    private readonly string[] Images = new[]
    {
        "Images\\Dishwasher.jpg",
        "Images\\Microwave.jpg",
        "Images\\Ninja.jpg",
        "Images\\Oven.jpg",
        "Images\\Refrigirator.jpg",
    };

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
        Id = Indexer();
        Image = Images[Id];
        Title = GetTitleFromImage(Image);
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