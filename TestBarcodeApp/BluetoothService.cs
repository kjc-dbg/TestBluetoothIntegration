//using Newtonsoft.Json;
//using Plugin.BLE;
//using Plugin.BLE.Abstractions.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TestBarcodeApp.Models;

//namespace TestBarcodeApp
//{
//    public class BluetoothService
//    {
//        private IBluetoothLE _ble;
//        private IAdapter _adapter;
//        private ICharacteristic _characteristic;
//        private IService _service;
//        private bool _isConnected = false;

//        // Replace with your PC's Bluetooth service UUID
//        private readonly string SERVICE_UUID = "12345678-1234-5678-9012-123456789abc";
//        private readonly string CHARACTERISTIC_UUID = "87654321-4321-8765-2109-cba987654321";

//        public BluetoothService()
//        {
//            _ble = CrossBluetoothLE.Current;
//            _adapter = _ble.Adapter;
//        }

//        public async Task<bool> ConnectToPC()
//        {
//            try
//            {
//                var scanResult = await ScanForDevices();
//                if (scanResult != null)
//                {
//                    await _adapter.ConnectToDeviceAsync(scanResult);
//                    _service = await scanResult.GetServiceAsync(Guid.Parse(SERVICE_UUID));
//                    _characteristic = await _service.GetCharacteristicAsync(Guid.Parse(CHARACTERISTIC_UUID));
//                    _isConnected = true;
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Bluetooth connection error: {ex.Message}");
//            }
//            return false;
//        }

//        private async Task<IDevice> ScanForDevices()
//        {
//            var devices = new List<IDevice>();

//            _adapter.DeviceAdvertised += (s, a) =>
//            {
//                //if (!string.IsNullOrEmpty(a.Device.Name) &&
//                //    a.Device.Name.Contains("KARTIKEYA"))
//                //{
//                    devices.Add(a.Device);


//                    System.Diagnostics.Debug.WriteLine($"Found device: {a.Device.Name} - {a.Device.Id}");

//                //}
//            };

//            await _adapter.StartScanningForDevicesAsync();
//            await Task.Delay(5000); // Scan for 5 seconds
//            await _adapter.StopScanningForDevicesAsync();

//            return devices.FirstOrDefault();
//        }

//        public async Task<bool> SendBarcodeData(BarcodeData data)
//        {
//            if (!_isConnected || _characteristic == null)
//                return false;

//            try
//            {
//                var json = JsonConvert.SerializeObject(data);
//                var bytes = Encoding.UTF8.GetBytes(json);
//                await _characteristic.WriteAsync(bytes);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Send data error: {ex.Message}");
//                return false;
//            }
//        }

//        public void Disconnect()
//        {
//            _isConnected = false;
//            // Cleanup connection
//        }
//    }
//}
