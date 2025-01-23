using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;

namespace CAMAUIGardenCentreApp
{
    public partial class App : Application
    {
        public App(DatabaseInitializer initializer)
        {
            InitializeComponent();

            MainPage = new SplashPage(initializer);
        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}