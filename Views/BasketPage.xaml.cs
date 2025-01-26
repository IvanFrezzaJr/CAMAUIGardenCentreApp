using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

public partial class BasketPage : ContentPage
{
    public BasketPage(BasketViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}