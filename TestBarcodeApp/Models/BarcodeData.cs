using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBarcodeApp.Models
{
    public class BarcodeData
    {
        public string Value { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string DeviceId { get; set; } = string.Empty;
    }
}
