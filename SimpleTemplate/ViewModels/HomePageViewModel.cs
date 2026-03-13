using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Views;

namespace SimpleTemplate.ViewModels
{
    [RegisterView(typeof(HomePageView))]
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
