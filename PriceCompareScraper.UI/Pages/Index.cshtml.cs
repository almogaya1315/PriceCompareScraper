using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PriceCompareScraper.UI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public readonly string[] Images = new[] 
    {
        "Images\\Dishwasher.jpg",
        "Images\\Microwave.jpg",
        "Images\\Ninja.jpg",
        "Images\\Oven.jpg",
        "Images\\Refrigirator.jpg",
    };

    [BindProperty(SupportsGet = true)]
    public int Index { get; set; } = 0;



    public void OnGet()
    {

    }
}
