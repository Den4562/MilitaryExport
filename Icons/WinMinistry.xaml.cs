using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppMilitaryExport.Icons;
using WpfAppMilitaryExport.Navigator;
using WpfAppMilitaryExport.DataBase.Table;
using WpfAppMilitaryExport.DB;

namespace WpfAppMilitaryExport.Icons
{
    /// <summary>
    /// Логика взаимодействия для Ministry.xaml
    /// </summary>
    public partial class WinMinistry : UserControl
    {
        public WinMinistry()
        {
            InitializeComponent();
       }

        private void LoadNavyDetails()
        {
            using (var context = new MilitaryDBContext())
            {
                var navyDetails = context.Navy_Details.ToList();
                DataTable.ItemsSource = navyDetails;
            }
        }

        private void bt_showDetails(object sender, RoutedEventArgs e)
        {
            LoadNavyDetails();
        }

        private void Click_Exit(object sender, RoutedEventArgs e)
        {
            var win_Main = new MainWindow();
            NavigatorObject.Switch(win_Main);
        }
    }
}
