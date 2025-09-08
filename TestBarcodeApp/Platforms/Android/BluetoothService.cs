using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.Bluetooth;
using Java.IO;
using Java.Util;
using System.Text;

namespace TestBarcodeApp.Platforms.Android
{
    public class BluetoothService
    {
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothSocket _socket;
        private OutputStreamWriter _writer;
        private readonly UUID SPP_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

        public event EventHandler<string> ConnectionStatusChanged;

        public BluetoothService()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public bool IsBluetoothEnabled => _bluetoothAdapter?.IsEnabled ?? false;

        public List<BluetoothDeviceInfo> GetPairedDevices()
        {
            var devices = new List<BluetoothDeviceInfo>();

            if (_bluetoothAdapter != null)
            {
                var pairedDevices = _bluetoothAdapter.BondedDevices;
                if (pairedDevices != null)
                {
                    foreach (var device in pairedDevices)
                    {
                        devices.Add(new BluetoothDeviceInfo
                        {
                            Name = device.Name ?? "Unknown",
                            Address = device.Address
                        });
                    }
                }
            }

            return devices;
        }

        public async Task<bool> ConnectAsync(string deviceAddress)
        {
            try
            {
                var device = _bluetoothAdapter.GetRemoteDevice(deviceAddress);
                _socket = device.CreateRfcommSocketToServiceRecord(SPP_UUID);

                await _socket.ConnectAsync();

                var outputStream = _socket.OutputStream;
                _writer = new OutputStreamWriter(outputStream);

                ConnectionStatusChanged?.Invoke(this, "Connected");
                return true;
            }
            catch (Exception ex)
            {
                ConnectionStatusChanged?.Invoke(this, $"Connection failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendBarcodeAsync(string barcode)
        {
            if (_socket == null || !_socket.IsConnected || _writer == null)
            {
                return false;
            }

            try
            {
                await _writer.WriteAsync(barcode + "\n");
                await _writer.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                ConnectionStatusChanged?.Invoke(this, $"Send failed: {ex.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                _writer?.Close();
                _socket?.Close();
                ConnectionStatusChanged?.Invoke(this, "Disconnected");
            }
            catch { }
            finally
            {
                _writer = null;
                _socket = null;
            }
        }
    }

    public class BluetoothDeviceInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
