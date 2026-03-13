using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Contracts.Services
{
    public interface IViewFactory
    {
        Page CreateView(Type viewType);
        object CreateViewModel(Type viewModelType);
    }
}
