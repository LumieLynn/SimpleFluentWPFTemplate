using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Views;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace SimpleTemplate.ViewModels
{
    [RegisterView(typeof(AppsPageView), ServiceLifetime.Transient, ServiceLifetime.Singleton)]
    public partial class AppsPageViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ObservableCollection<Brush> _buttonSource = new();

        public AppsPageViewModel()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            for (int i = 0; i < 2048; i++)
            {
                var brush = new SolidColorBrush(
                    Color.FromArgb(
                        (byte)200,
                        (byte)Random.Shared.Next(0, 250),
                        (byte)Random.Shared.Next(0, 250),
                        (byte)Random.Shared.Next(0, 250)
                        )
                    );
                brush.Freeze();
                ButtonSource.Add(brush);
            }
        }
    }
}
