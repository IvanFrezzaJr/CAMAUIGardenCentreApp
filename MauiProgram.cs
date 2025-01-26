﻿using CAMAUIGardenCentreApp.Services;
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
            builder.Services.AddSingleton<CartService>();
            builder.Services.AddSingleton<LoadingService>();

            // register ViewModels
            builder.Services.AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<CartViewModel>();
            builder.Services.AddTransient<CheckoutViewModel>();
            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<ProductListViewModel>();

            // register pages
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<CheckoutPage>();
            builder.Services.AddTransient<SuccessPage>();
            builder.Services.AddTransient<CategoryPage>();


            return builder.Build();
        }
    }
}
