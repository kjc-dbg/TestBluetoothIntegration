

#if ANDROID
using TestBarcodeApp.Platforms.Android;
#endif
using ZXing.Net.Maui;

namespace TestBarcodeApp
{
    public partial class MainPage : ContentPage
    {
        #if ANDROID
        private BluetoothService _bluetoothService;
        #endif

        private bool _isConnected = false;

        public MainPage()
        {
            InitializeComponent();
            InitializeBluetooth();
        }

        private void InitializeBluetooth()
        {
#if ANDROID
            _bluetoothService = new BluetoothService();
            _bluetoothService.ConnectionStatusChanged += OnConnectionStatusChanged;
            LoadPairedDevices();
#endif
        }

        private void LoadPairedDevices()
        {
#if ANDROID
            var devices = _bluetoothService.GetPairedDevices();
            DevicePicker.ItemsSource = devices;
#endif
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
#if ANDROID
            if (_isConnected)
            {
                _bluetoothService.Disconnect();
                UpdateUI(false);
            }
            else
            {
                var selectedDevice = DevicePicker.SelectedItem as BluetoothDeviceInfo;
                if (selectedDevice != null)
                {
                    var success = await _bluetoothService.ConnectAsync(selectedDevice.Address);
                    UpdateUI(success);
                }
                else
                {
                    await DisplayAlert("Error", "Please select a device", "OK");
                }
            }
#endif
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
            var scanPage = new BarcodeScannerPage();
            scanPage.BarcodeDetected += async (s, args) =>
            {
                var barcode = args.BarcodeResult.Value;
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    BarcodeEntry.Text = barcode;
                    await Navigation.PopAsync();

#if ANDROID
                    if (_isConnected)
                    {
                        await _bluetoothService.SendBarcodeAsync(barcode);
                        await DisplayAlert("Success", "Barcode sent!", "OK");
                    }
#endif
                });
            };

            await Navigation.PushAsync(scanPage);
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
#if ANDROID
            if (!string.IsNullOrWhiteSpace(BarcodeEntry.Text))
            {
                var success = await _bluetoothService.SendBarcodeAsync(BarcodeEntry.Text);
                if (success)
                {
                    await DisplayAlert("Success", "Barcode sent!", "OK");
                    BarcodeEntry.Text = "";
                }
                else
                {
                    await DisplayAlert("Error", "Failed to send barcode", "OK");
                }
            }
#endif
        }

        private void OnConnectionStatusChanged(object sender, string status)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                StatusLabel.Text = status;
            });
        }

        private void UpdateUI(bool connected)
        {
            _isConnected = connected;
            StatusLabel.Text = connected ? "Connected" : "Disconnected";
            StatusLabel.BackgroundColor = connected ? Colors.Green : Colors.Red;
            ConnectButton.Text = connected ? "Disconnect" : "Connect";
            ScanButton.IsEnabled = connected;
            SendButton.IsEnabled = connected;
        }
    }
}
