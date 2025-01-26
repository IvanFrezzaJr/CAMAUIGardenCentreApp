using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

public partial class CategoryPage : ContentPage
{
    private readonly CategoryViewModel _viewModel;
    public CategoryPage(CategoryViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCategoriesAsync();
    }
}