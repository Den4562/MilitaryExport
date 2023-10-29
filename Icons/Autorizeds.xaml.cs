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

      
        private async void Enter_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Pass.Password;

            
                bool isUserAuthenticated = await Authenticate(login, password);

                if (isUserAuthenticated)
                {
                    // Успешная аутентификация, переключитесь на окно Army_Request
                    var armyRequestWindow = new Army_Request(); // Предполагается, что это ваше окно Army_Request
                    NavigatorObject.Switch(armyRequestWindow);
                }
                else
                {
                    // Пользователь не найден, выполните действия по обработке ошибки
                    ShowSnackbar("Ошибка аутентификации");
                }
            
            
            

            Login.Text = "";
            Pass.Password = "";
        }

        private void ShowSnackbar(string message)
        {
            messageQueue.Enqueue(message);
        }

        private async Task<bool> Authenticate(string login, string password)
        {
            using (var context = new MilitaryDBContext())
            {
                // Проверяем, существует ли пользователь с указанным логином и паролем
                var user = await context.Account_Ministry.SingleOrDefaultAsync(u => u.Login == login && u.Password == password);

                return user != null;
            }
        }
    }
}
