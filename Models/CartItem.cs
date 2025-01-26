using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAMAUIGardenCentreApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CAMAUIGardenCentreApp.Models;

public partial  class CartItem : ObservableObject
{
    [ObservableProperty]
    private Product _product;

    [ObservableProperty]
    private int _quantity = 1;
}
