using System.Windows;

namespace Farmacia_app
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void OnOpenAdmin(object sender, RoutedEventArgs e)
        {
            var adminWindow = new MainWindow();
            adminWindow.Show();
            Close();
        }

        private void OnOpenClient(object sender, RoutedEventArgs e)
        {
            var clientWindow = new ClientWindow();
            clientWindow.Show();
            Close();
        }
    }
}
