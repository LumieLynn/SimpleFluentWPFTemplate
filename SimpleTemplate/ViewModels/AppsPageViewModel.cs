using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleTemplate.ViewModels
{
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
            for (int i = 0; i < 256; i++)
            {
                var random = new Random();
                ButtonSource.Add(
                    new SolidColorBrush(
                        Color.FromArgb(
                            (byte)200,
                            (byte)random.Next(0, 250),
                            (byte)random.Next(0, 250),
                            (byte)random.Next(0, 250)
                            )
                        )
                    );
            }
        }
    }
}
