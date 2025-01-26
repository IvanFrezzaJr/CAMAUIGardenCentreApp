using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

public partial class CheckoutPage : ContentPage
{
	public CheckoutPage(CheckoutViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}