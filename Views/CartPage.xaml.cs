using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

public partial class CartPage : ContentPage
{
    public CartPage(CartViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}