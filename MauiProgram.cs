using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CAMAUIGardenCentreApp.ViewModels;
using CAMAUIGardenCentreApp.Data;
using Microsoft.Extensions.Logging;

namespace CAMAUIGardenCentreApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // register services
            //builder.Services.AddSingleton<DatabaseService>();

            // register ViewModels
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<DatabaseInitializer>();
            builder.Services.AddSingleton<ProductsViewModel>();

            // register pages
            builder.Services.AddTransient<AuthService>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProfilePage>();


            return builder.Build();
        }
    }
}
