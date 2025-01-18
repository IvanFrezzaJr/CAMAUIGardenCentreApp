using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace CAMAUIGardenCentreApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private int count = 0;

        [ObservableProperty]
        private string counterText = "Clicked 0 times";

        [RelayCommand]
        private void CounterClicked()
        {
            count++;
            CounterText = count == 1 ? $"Clicked {count} time" : $"Clicked {count} times";
            Debug.WriteLine(CounterText);
            SemanticScreenReader.Announce(CounterText);
        }
    }
}
