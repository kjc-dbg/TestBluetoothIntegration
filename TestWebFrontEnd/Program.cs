using TestWebFrontEnd;
using TestWebFrontEnd.Hubs;
using TestWebFrontEnd.Service;
using static TestWebFrontEnd.Hubs.BarcodeHubService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSingleton<BarcodeDataService>();
builder.Services.AddSingleton<MyBluetoothListener>();
builder.Services.AddHostedService<BluetoothBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();
app.MapHub<BarcodeHub>("/barcodeHub");

app.Run();

public class BluetoothBackgroundService : BackgroundService
{
    private readonly MyBluetoothListener _bluetoothListener;
    private readonly BarcodeDataService _barcodeService;

    public BluetoothBackgroundService(MyBluetoothListener bluetoothListener, BarcodeDataService barcodeService)
    {
        _bluetoothListener = bluetoothListener;
        _barcodeService = barcodeService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bluetoothListener.BarcodeReceived += async (sender, barcode) =>
        {
            await _barcodeService.AddBarcodeAsync(barcode);
        };

        await _bluetoothListener.StartListeningAsync();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _bluetoothListener.StopListening();
        await base.StopAsync(cancellationToken);
    }
}