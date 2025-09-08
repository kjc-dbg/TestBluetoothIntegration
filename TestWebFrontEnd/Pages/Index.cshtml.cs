using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static TestWebFrontEnd.Hubs.BarcodeHubService;

namespace TestWebFrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BarcodeDataService _barcodeService;

        public List<string> Barcodes { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, BarcodeDataService barcodeService)
        {
            _logger = logger;
            _barcodeService = barcodeService;
        }

        public void OnGet()
        {
            Barcodes = _barcodeService.GetRecentBarcodes().ToList();
        }
    }
}
