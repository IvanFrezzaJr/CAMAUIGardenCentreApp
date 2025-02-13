using CAMAUIGardenCentreApp.ViewModels;

namespace CAMAUIGardenCentreApp.Views;

[QueryProperty(nameof(ProductId), "product_id")]
public partial class DetailPage : ContentPage
{
    private readonly DetailViewModel _viewModel;

    private int _productId;
    public int ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            LoadProduct(_productId);
        }
    }

    public DetailPage(DetailViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }


    private void LoadProduct(int productId)
    {
        _viewModel.LoadProductByIdAsync(productId);
    }
}