using System;
using System.Windows;
using System.Windows.Controls;
using WpfAppMilitaryExport.Navigator;
using WpfAppMilitaryExport.Icons;
namespace WpfAppMilitaryExport
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            NavigatorObject.pageSwitcher = this;
            NavigatorObject.Switch(new MainWindow());
        }

        public Action? CloseAction { get; set; }

        public void Navigate(UserControl nexPage)
        {
            this.Content = nexPage;
        }

        public void Navigate(UserControl nexPage, object state)
        {
            this.Content = nexPage;
            //INavigator? s = nextPage as INavigator;

            //if (s != null)
            //    s.UtilizeState(state);
            //else
            //    throw new ArgumentException("NextPage is not INavigator!" + nexPage.Name.ToString());
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
    }
}
