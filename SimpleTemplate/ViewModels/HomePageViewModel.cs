using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Views;

namespace SimpleTemplate.ViewModels
{
    [RegisterView(typeof(HomePageView), ServiceLifetime.Transient, ServiceLifetime.Singleton)]
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
