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

    public string CurrentImage => Images[Indexer()];

    public int Count => Images.Length;

    public void OnGet()
    {
        Index = Indexer();
    }

    private int Indexer()
    {
        return Index < 0                            // if index below first
                     ? 0                            // limit to first (0)
                     : (Index >= Images.Length      // else, if index beyond last
                     ? Images.Length - 1            // limit to last (Length-1)
                     : Index);                      // else, keep index as-is
    }
}
