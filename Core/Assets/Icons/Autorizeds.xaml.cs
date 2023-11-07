using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfAppMilitaryExport.DB;
using WpfAppMilitaryExport.Navigator;

namespace WpfAppMilitaryExport.Icons
{
    /// <summary>
    /// Логика взаимодействия для Autorizeds.xaml
    /// </summary>
    public partial class Autorizeds : UserControl
    {
        private SnackbarMessageQueue messageQueue = new SnackbarMessageQueue();
        public Autorizeds()
        {
            InitializeComponent();
            Snackbar.MessageQueue = messageQueue;

        }

        private void bt_MainClick(object sender, RoutedEventArgs e)
        {
            var exit = new MainWindow();
            NavigatorObject.Switch(exit);
        }



        private async void Enter_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Pass.Password;

            bool MinAuthenticated = await AuthenticateMinistry(login, password);
            bool CommAuthenticated = await AuthenticateCommand(login, password);

            if (MinAuthenticated || CommAuthenticated)
            {
                if (MinAuthenticated)
                {
                  
                    var ministryWindow = new WinMinistry(); // 
                    NavigatorObject.Switch(ministryWindow);
                }
                else
                {
                    // Успешная аутентификация в таблице Account_Command
                    var commandWindow = new Army_Request();
                    NavigatorObject.Switch(commandWindow);
                }
            }
            else
            {
                // Ошибка аутентификации
                ShowSnackbar("Помилка автентифікація");
            }

            Login.Text = "";
            Pass.Password = "";
        }

        private void ShowSnackbar(string message)
        {
            messageQueue.Enqueue(message);
        }

        private async Task<bool> AuthenticateMinistry(string login, string password)
        {
            using (var context = new MilitaryDBContext())
            {
                var user = await context.Account_Ministry.SingleOrDefaultAsync(u => u.Login == login && u.Password == password);

                return user != null;
            }
        }

        private async Task<bool> AuthenticateCommand(string login, string password)
        {
            using (var context = new MilitaryDBContext())
            {
                
                var user = await context.Account_Command.SingleOrDefaultAsync(u => u.Login == login && u.Password == password);

                return user != null;
            }
        }
    }
}
