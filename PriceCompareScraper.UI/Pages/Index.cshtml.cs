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
    // TODO: Validation & Security on client, server & DB
    // TODO: User & Traffic authentication & storing
    // TODO: Traffic capture & statistics
    // TODO: Add & banners


    public IndexModel(ILogger<IndexModel> logger) : base(logger)
    {
        // ctor
    }

    public string[] Images;

    [BindProperty(SupportsGet = true)]
    public int Index { get; set; } = 0;

    public string CurrentImage => Images[Indexer(Images, Index)];

    public int Count => Images.Length;

    public void OnGet()
    {
        SetContext(HttpContext);
        Images = SetImages(init: true);
        Index = Indexer(Images, Index);
    }
}
