using SimpleTemplate.Views;
using System.Windows;

namespace SimpleTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationRootView _rootView;

        public MainWindow(NavigationRootView rootView)
        {
            _rootView = rootView;

            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Content = _rootView;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
