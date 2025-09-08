using InTheHand.Net.Sockets;
using TestWebFrontEnd.Hubs;
using TestWebFrontEnd.Models;

namespace TestWebFrontEnd.Service
{
    public class BluetoothHostedService //: IHostedService
    {
        //private MyBluetoothListener _bluetoothListener;
        //private readonly IBarcodeHub _barcodeHub;
        //private readonly ILogger<BluetoothHostedService> _logger;

        //public BluetoothHostedService(IBarcodeHub barcodeHub, ILogger<BluetoothHostedService> logger)
        //{
        //    _barcodeHub = barcodeHub;
        //    _logger = logger;
        //}

        //public async Task StartAsync(CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("Starting Bluetooth service...");

        //    _bluetoothListener = new MyBluetoothListener();
        //    _bluetoothListener.BarcodeReceived += OnBarcodeReceived;

        //    await _bluetoothListener.StartListeningAsync();
        //}

        //public Task StopAsync(CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("Stopping Bluetooth service...");

        //    _bluetoothListener?.StopListening();
        //    return Task.CompletedTask;
        //}

        //private async void OnBarcodeReceived(object sender, string barcode)
        //{
        //    _logger.LogInformation($"Barcode received: {barcode}");

        //    var barcodeData = new BarcodeData
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        Value = barcode,
        //        ReceivedAt = DateTime.Now
        //    };

        //    await _barcodeHub.SendBarcodeAsync(barcodeData);
        //}
    }
}
