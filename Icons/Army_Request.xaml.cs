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

        private void MenuNavyWeapon_Click(object sender, RoutedEventArgs e)
        {
            var win_NavyWeapon = new WinNavyWeapon();
            NavigatorObject.Switch(win_NavyWeapon);
        }

        private void MenuNavyDetails_Click(object sender, RoutedEventArgs e)
        {
            var win_NavyDetails = new WinNavyDetails();
            NavigatorObject.Switch(win_NavyDetails);
        }


        private void Click_Exit(object sender, RoutedEventArgs e)
        {
            var win_Autorized = new Autorizeds();
            NavigatorObject.Switch(win_Autorized);
        }

        private void MenuDetails_Click(object sender, RoutedEventArgs e)
        {
            var win_details = new WinDetails();
            NavigatorObject.Switch(win_details);
        }

        private void MenuAmmo_Click(object sender, RoutedEventArgs e)
        {
            var win_ammo = new WinAmmo();
            NavigatorObject.Switch(win_ammo);
        }

        private void MenuInfarny_Click(object sender, RoutedEventArgs e)
        {
            var win_infarny = new WinInfarny();
            NavigatorObject.Switch(win_infarny);
        }
        private void MenuWeapon_Click(object sender, RoutedEventArgs e)
        {
            var win_weapon = new WinWeapon();
            NavigatorObject.Switch(win_weapon);
        }

        private void CreateQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-N5K3CGS\\SQLEXPRESS01;Initial Catalog=MilitaryExport;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    connection.Open();


                    int Air_forces_requestId;
                    int Ground_forces_requestId;
                    int Navy_forces_requestId;


                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Air_forces_request ORDER BY Id DESC", connection))
                    {
                        Air_forces_requestId = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Ground_forces_request ORDER BY Id DESC", connection))
                    {
                        Ground_forces_requestId = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Navy_forces_request ORDER BY Id DESC", connection))
                    {
                        Navy_forces_requestId = (int)cmd.ExecuteScalar();
                    }



                    string insertQuery = "INSERT INTO Army_Order (Ground_forces_requestId,Air_forces_requestId, Navy_forces_requestId,Cost) " +
                                         "VALUES (@Ground_forces_requestId, @Air_forces_requestId, @Navy_forces_requestId,0)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Ground_forces_requestId", Ground_forces_requestId);
                        cmd.Parameters.AddWithValue("@Air_forces_requestId", Air_forces_requestId);
                        cmd.Parameters.AddWithValue("@Navy_forces_requestId", Navy_forces_requestId);

                        //// Выполните SQL-запрос
                        cmd.ExecuteNonQuery();
                        string triggerQuery = "UPDATE Army_Order SET Cost = 0 WHERE Id = SCOPE_IDENTITY()"; // Используйте SCOPE_IDENTITY() для получения ID только что вставленной записи
                        using (SqlCommand triggerCmd = new SqlCommand(triggerQuery, connection))
                        {
                            triggerCmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Запись успешно создана.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании записи: " + ex.Message);
            }
        }

    }
}
