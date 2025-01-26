using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAMAUIGardenCentreApp.Views;

namespace CAMAUIGardenCentreApp.Services;

using System.Threading.Tasks;

public class LoadingService
{
    public async Task ShowLoadingWhile(Func<Task> action)
    {
        await Shell.Current.GoToAsync(nameof(LoadingPage));

        await action();

        // go back to de previous page
        await Shell.Current.GoToAsync(".."); 
    }
}

