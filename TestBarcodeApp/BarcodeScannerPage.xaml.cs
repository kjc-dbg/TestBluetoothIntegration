using ZXing.Net.Maui;

namespace TestBarcodeApp;

public partial class BarcodeScannerPage : ContentPage
{
    public event EventHandler<BarcodeDetectedEventArgs> BarcodeDetected;

    public BarcodeScannerPage()
    {
        InitializeComponent();

        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            Multiple = false,
            AutoRotate = true,
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        cameraBarcodeReaderView.BarcodesDetected += OnBarcodesDetected;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraBarcodeReaderView.BarcodesDetected -= OnBarcodesDetected;
    }

    void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        try
        {
            if (e.Results.Any())
            {
                var first = e.Results.FirstOrDefault();
                BarcodeDetected?.Invoke(this, new BarcodeDetectedEventArgs { BarcodeResult = first });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}

public class BarcodeDetectedEventArgs : EventArgs
{
    public BarcodeResult BarcodeResult { get; set; }
}