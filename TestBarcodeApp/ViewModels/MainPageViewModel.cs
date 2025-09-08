//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using TestBarcodeApp.Models;
//using ZXing.Net.Maui;

//namespace TestBarcodeApp.ViewModels
//{
//    public class MainPageViewModel : INotifyPropertyChanged
//    {
//        private readonly BluetoothService _bluetoothService;
//        private bool _isConnected = false;
//        private string _lastScannedCode = string.Empty;
//        private string _connectionStatus = "Disconnected";

//        public MainPageViewModel(BluetoothService bluetoothService)
//        {
//            _bluetoothService = bluetoothService;
//            ConnectCommand = new Command(async () => await ConnectToPCAsync());
//            BarcodeDetectedCommand = new Command<BarcodeDetectionEventArgs>(OnBarcodeDetected);
//        }

//        public ICommand ConnectCommand { get; }
//        public ICommand BarcodeDetectedCommand { get; }

//        public bool IsConnected
//        {
//            get => _isConnected;
//            set
//            {
//                _isConnected = value;
//                OnPropertyChanged();
//                ConnectionStatus = value ? "Connected" : "Disconnected";
//            }
//        }

//        public string LastScannedCode
//        {
//            get => _lastScannedCode;
//            set
//            {
//                _lastScannedCode = value;
//                OnPropertyChanged();
//            }
//        }

//        public string ConnectionStatus
//        {
//            get => _connectionStatus;
//            set
//            {
//                _connectionStatus = value;
//                OnPropertyChanged();
//            }
//        }

//        private async Task ConnectToPCAsync()
//        {
//            await RequestBluetoothScanPermission();
//            ConnectionStatus = "Connecting...";
//            IsConnected = await _bluetoothService.ConnectToPC();
//        }



//        public async Task<PermissionStatus> RequestBluetoothScanPermission()
//        {
//            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();

//            if (status != PermissionStatus.Granted)
//            {
//                status = await Permissions.RequestAsync<Permissions.Bluetooth>();
//            }

//            return status;
//        }

//        private async void OnBarcodeDetected(BarcodeDetectionEventArgs e)
//        {
//            if (e.Results?.FirstOrDefault() is BarcodeResult result)
//            {
//                var barcodeData = new BarcodeData
//                {
//                    Value = result.Value,
//                    Format = result.Format.ToString(),
//                    DeviceId = DeviceInfo.Current.Model
//                };

//                LastScannedCode = result.Value;

//                if (IsConnected)
//                {
//                    await _bluetoothService.SendBarcodeData(barcodeData);
//                }
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
