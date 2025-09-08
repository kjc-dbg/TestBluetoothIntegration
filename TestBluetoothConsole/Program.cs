using TestBluetoothConsole;

// Create and configure the Bluetooth listener
var bluetoothService = new MyBluetoothListener();

// Subscribe to barcode received events
bluetoothService.BarcodeReceived += (sender, barcode) =>
{
    Console.WriteLine($"Scanned barcode: {barcode}");
    // Process your barcode data here
};

try
{
    Console.WriteLine("Starting Bluetooth listener...");
    await bluetoothService.StartListeningAsync();
    
    Console.WriteLine("Press 'q' to quit or any other key to continue...");
    
    // Keep the application running
    while (true)
    {
        var key = Console.ReadKey(true);
        if (key.KeyChar == 'q' || key.KeyChar == 'Q')
        {
            break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    // Clean shutdown
    bluetoothService.StopListening();
    Console.WriteLine("Application stopped.");
}
