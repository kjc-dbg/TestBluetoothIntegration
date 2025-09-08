using InTheHand.Net.Sockets;
using System.Text;

namespace TestWebFrontEnd
{
    public class MyBluetoothListener
    {
        private BluetoothListener _listener;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Guid _serviceGuid = new Guid("00001101-0000-1000-8000-00805F9B34FB"); // SPP UUID

        public event EventHandler<string> BarcodeReceived;

        public async Task StartListeningAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Initialize Bluetooth listener
                _listener = new BluetoothListener(_serviceGuid);
                _listener.Start();

                Console.WriteLine("Bluetooth service started. Waiting for connections...");

                // Accept connections in background
                _ = Task.Run(async () => await AcceptConnectionsAsync(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Bluetooth listener: {ex.Message}");
                throw;
            }
        }

        private async Task AcceptConnectionsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Wait for client connection
                    var client = await Task.Run(() => _listener.AcceptBluetoothClient(), cancellationToken);
                    Console.WriteLine($"Connected to: {client.RemoteMachineName}");

                    // Handle client in separate task
                    _ = Task.Run(async () => await HandleClientAsync(client, cancellationToken));
                }
                catch (ObjectDisposedException)
                {
                    // Listener was stopped
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting connection: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(BluetoothClient client, CancellationToken cancellationToken)
        {
            try
            {
                using (client)
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    while (client.Connected && !cancellationToken.IsCancellationRequested)
                    {
                        var data = await reader.ReadLineAsync();
                        if (data != null)
                        {
                            Console.WriteLine($"Received barcode: {data}");
                            BarcodeReceived?.Invoke(this, data);
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Client disconnected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }

        public void StopListening()
        {
            _cancellationTokenSource?.Cancel();
            _listener?.Stop();
            Console.WriteLine("Bluetooth service stopped");
        }
    }
}
