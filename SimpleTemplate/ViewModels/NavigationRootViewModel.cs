using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleTemplate.ViewModels
{
    public partial class NavigationRootViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private bool isBackEnabled;

        [ObservableProperty]
        private string _appTitle = "SimpleTemplate";

        [ObservableProperty]
        private string _paneTitle = "Samples";

        [ObservableProperty]
        private object? selected;

    }
}
