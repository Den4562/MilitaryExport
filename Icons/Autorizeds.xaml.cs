using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
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

            bool isMinistryAuthenticated = await AuthenticateMinistry(login, password);
            bool isCommandAuthenticated = await AuthenticateCommand(login, password);

            if (isMinistryAuthenticated || isCommandAuthenticated)
            {
                if (isMinistryAuthenticated)
                {
                    // Успешная аутентификация в таблице Account_Ministry
                    var ministryWindow = new WinMinistry(); // Предполагается, что это ваше окно для Ministry
                    NavigatorObject.Switch(ministryWindow);
                }
                else
                {
                    // Успешная аутентификация в таблице Account_Command
                    var commandWindow = new Army_Request(); // Предполагается, что это ваше окно для Command
                    NavigatorObject.Switch(commandWindow);
                }
            }
            else
            {
                // Ошибка аутентификации
                ShowSnackbar("Ошибка аутентификации");
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
