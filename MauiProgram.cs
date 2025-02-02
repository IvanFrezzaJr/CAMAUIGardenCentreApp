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
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<DatabaseInitializer>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<BasketService>();
            builder.Services.AddSingleton<LoadingService>();
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<RegisterService>();

            // register ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<BasketViewModel>();
            builder.Services.AddTransient<CheckoutViewModel>();
            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<ProductListViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();

            // register pages
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<CheckoutPage>();
            builder.Services.AddTransient<SuccessPage>();
            builder.Services.AddTransient<CategoryPage>();


            return builder.Build();
        }
    }
}
