using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdditionalProject.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void ManageListView_Navigate(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy napewno chcesz przejść do list? Aktualny widok losowania ulegnie zapisowi i wyczyszczeniu widoku.", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    MainFrame.Navigate(new ManageListView(this));
                    break;
                case MessageBoxResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        private void RandomizeView_Navigate(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RandomizeView());
        }
    }
}
