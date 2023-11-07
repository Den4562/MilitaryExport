using System.Windows.Controls;


namespace WpfAppMilitaryExport.Navigator
{
    public class NavigatorObject
    {
        public static Main? pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            pageSwitcher?.Navigate(newPage);
        }

        public static void Switch(UserControl newPage, object state)
        {
            pageSwitcher?.Navigate(newPage, state);
        }
    }
}
