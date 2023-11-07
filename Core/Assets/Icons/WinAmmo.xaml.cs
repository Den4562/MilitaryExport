using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfAppMilitaryExport.DataBase.Table;
using WpfAppMilitaryExport.DB;
using WpfAppMilitaryExport.Navigator;

namespace WpfAppMilitaryExport.Icons
{
    /// <summary>
    /// Логика взаимодействия для WinAmmo.xaml
    /// </summary>
    public partial class WinAmmo : UserControl
    {
        public WinAmmo()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MilitaryDBContext())
            {
           
                await context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER UpdateAmmoTotalCost ON Ammo");

              
                var newAmmo = new Ammo
                {
                    Name = txtName.Text,
                    Count = int.Parse(txtCount.Text),
                    Unit_Cost = decimal.Parse(txtUnitCost.Text),
                    Total_Cost = int.Parse(txtCount.Text) * decimal.Parse(txtUnitCost.Text)
                };

            
                context.Ammo.Add(newAmmo);
                context.SaveChanges();

              
                await context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER UpdateAmmoTotalCost ON Ammo");

              
                txtName.Clear();
                txtCount.Clear();
                txtUnitCost.Clear();

            
            }


        }

        private void bt_AirClick(object sender, RoutedEventArgs e)
        {
            var win_air = new WinAir();
            NavigatorObject.Switch(win_air);
        }


        private void click_Main(object sender, RoutedEventArgs e)
        {
            var main = new Army_Request();
            NavigatorObject.Switch(main);
        }

        private void click_Exit(object sender, RoutedEventArgs e)
        {
            var exit = new MainWindow();
            NavigatorObject.Switch(exit);
        }

        private void bt_DetailsClick(object sender, RoutedEventArgs e)
        {
            var win_details = new WinDetails();
            NavigatorObject.Switch(win_details);
        }

        private void CreateQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создайте подключение к базе данных (используйте свой метод подключения)
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-N5K3CGS\\SQLEXPRESS01;Initial Catalog=MilitaryExport;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    connection.Open();

                    // Получите последние значения AirplaneId, AmmoId и DetailsId из таблиц Airplane, Ammo и Details
                    int selectedAirplaneId;
                    int selectedAmmoId;
                    int selectedDetailsId;

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Airplane ORDER BY Id DESC", connection))
                    {
                        selectedAirplaneId = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Ammo ORDER BY Id DESC", connection))
                    {
                        selectedAmmoId = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Details ORDER BY Id DESC", connection))
                    {
                        selectedDetailsId = (int)cmd.ExecuteScalar();
                    }

                    // Создайте SQL-запрос для вставки новой записи в таблицу Air_forces_request
                    string insertQuery = "INSERT INTO Air_forces_request (AirplaneId, AmmoId, DetailsId, Cost) " +
                                         "VALUES (@AirplaneId, @AmmoId, @DetailsId, 0)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@AirplaneId", selectedAirplaneId);
                        cmd.Parameters.AddWithValue("@AmmoId", selectedAmmoId);
                        cmd.Parameters.AddWithValue("@DetailsId", selectedDetailsId);


                        // Выполните SQL-запрос
                        cmd.ExecuteNonQuery();
                        string triggerQuery = "UPDATE Air_forces_request SET Cost = 0 WHERE Id = SCOPE_IDENTITY()"; // Используйте SCOPE_IDENTITY() для получения ID только что вставленной записи
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
        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem selectedItem)
            {
                // Получите текст выбранного элемента и установите его в поле txtName
                txtName.Text = selectedItem.Header.ToString();
            }
        }
    }
}
