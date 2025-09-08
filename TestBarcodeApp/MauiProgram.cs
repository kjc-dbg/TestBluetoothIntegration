using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace TestBarcodeApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            //builder.Services.AddSingleton<BluetoothService>();
            //builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();



            var a = builder.Build();

            //we must initialize our service helper before using it
            ServiceHelper.Initialize(a.Services);


            return a;
        }
    }
}
