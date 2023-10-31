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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppMilitaryExport.Icons;
using WpfAppMilitaryExport.Navigator;

namespace WpfAppMilitaryExport
{
    /// <summary>
    /// Логика взаимодействия для Army_Request.xaml
    /// </summary>
    public partial class Army_Request : UserControl
    {
        public Army_Request()
        {
            InitializeComponent();
        }

        private void MenuAir_Click(object sender, RoutedEventArgs e)
        {
            var win_air = new WinAir();
            NavigatorObject.Switch(win_air);
        }
    }
}
