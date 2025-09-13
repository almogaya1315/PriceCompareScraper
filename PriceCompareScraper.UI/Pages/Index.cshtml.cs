using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PriceCompareScraper.Core.Bases;
using PriceCompareScraper.Core.Enums;
using PriceCompareScraper.Core.Services;
using PriceCompareScraper.UI.Pages.Shared;
using System.Text.Json.Serialization;

namespace PriceCompareScraper.UI.Pages;

public class IndexModel : ModelBase<IndexModel>
{
    public IndexModel(ILogger<IndexModel> logger) : base(logger)
    {
        // ctor
    }

    public string[] Images;

    [BindProperty(SupportsGet = true)]
    public int Index { get; set; } = 0;

    public string CurrentImage => Images[Indexer()];

    public int Count => Images.Length;

    public void OnGet()
    {
        SetHttpContext();
        SetImages();
        Index = Indexer();
    }

    private void SetHttpContext()
    {
        if (_session is null)
        {
            _session = new SessionHandler(HttpContext);
        }
        else
        {

        }
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
            Images =
            [
                "Images\\Dishwasher.jpg",
                "Images\\Microwave.jpg",
                "Images\\Ninja.jpg",
                "Images\\Oven.jpg",
                "Images\\Refrigirator.jpg",
            ];
            imagesJson = JsonConvert.SerializeObject(Images);
            _session.Set(eSessionKeys.Images, imagesJson);
        }
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
