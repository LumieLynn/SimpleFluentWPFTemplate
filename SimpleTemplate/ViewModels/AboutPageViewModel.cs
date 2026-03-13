using CommunityToolkit.Mvvm.ComponentModel;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Views;

namespace SimpleTemplate.ViewModels
{
    [RegisterView(typeof(AboutPageView))]
    public partial class AboutPageViewModel : ObservableRecipient
    {

    }
}
