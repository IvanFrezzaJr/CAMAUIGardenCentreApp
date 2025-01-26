using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

[QueryProperty(nameof(CategoryId), "category_id")]
public partial class ProductListPage : ContentPage
{

    private readonly ProductListViewModel _viewModel;

    private int _categoryId;
    public int CategoryId
    {
        get => _categoryId;
        set
        {
            _categoryId = value;
            LoadProducts(_categoryId);
        }
    }

    public ProductListPage(ProductListViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

    }

    private void LoadProducts(int categoryId)
    {
        _viewModel.LoadProductsByCategoryAsync(categoryId);
    }
}