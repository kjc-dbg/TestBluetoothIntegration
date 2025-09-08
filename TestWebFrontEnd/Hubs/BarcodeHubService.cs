using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using TestWebFrontEnd.Models;

namespace TestWebFrontEnd.Hubs
{
    //public interface IBarcodeHub
    //{
    //    Task SendBarcodeAsync(BarcodeData barcodeData);
    //}

    public class BarcodeHubService
    {
        public class BarcodeDataService
        {
            private readonly ConcurrentQueue<string> _barcodes = new();
            private readonly IHubContext<BarcodeHub> _hubContext;
            public event EventHandler<string> BarcodeAdded;

            public BarcodeDataService(IHubContext<BarcodeHub> hubContext)
            {
                _hubContext = hubContext;
            }

            public async Task AddBarcodeAsync(string barcode)
            {
                _barcodes.Enqueue(barcode);
                BarcodeAdded?.Invoke(this, barcode);

                // Send to all connected clients via SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveBarcode", barcode);
            }

            public IEnumerable<string> GetRecentBarcodes(int count = 10)
            {
                return _barcodes.TakeLast(count);
            }
        }
    }

    public class BarcodeHub : Hub
    {
        public async Task SendBarcode(string barcode)
        {
            await Clients.All.SendAsync("ReceiveBarcode", barcode);
        }
    }
}
