using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleTemplate.ViewModels
{
    public partial class HomePageViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private string _title = "Home";

        [ObservableProperty]
        private int _counter = 0;

        [RelayCommand]
        public void OnClicks()
        {
            Counter++;
        }
    }
}
