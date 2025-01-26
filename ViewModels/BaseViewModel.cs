using CAMAUIGardenCentreApp.Data;
using CAMAUIGardenCentreApp.Models;
using CAMAUIGardenCentreApp.Services;
using CAMAUIGardenCentreApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CAMAUIGardenCentreApp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText = "Processing...";

        /// <summary>
        /// Executa uma operação assíncrona com controle de estado de carregamento e tratamento de erros.
        /// </summary>
        protected async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            if (IsBusy) return; // Evita múltiplas execuções simultâneas

            IsBusy = true;
            BusyText = busyText ?? "Processing...";

            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }
    }
}
