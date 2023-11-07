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
    /// Логика взаимодействия для WinWeapon.xaml
    /// </summary>
    public partial class WinWeapon : UserControl
    {
        public WinWeapon()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MilitaryDBContext())
            {
    
                await context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER Infarny_WeaponTotalCost ON Infarny_Weapon");

                var newWeapon = new Infarny_Weapon()
                {
                    Name = txtName.Text,
                    Count = int.Parse(txtCount.Text),
                    Unit_Cost = decimal.Parse(txtUnitCost.Text),
                    Total_Cost = int.Parse(txtCount.Text) * decimal.Parse(txtUnitCost.Text)
                };

                context.Infarny_Weapon.Add(newWeapon);
                context.SaveChanges();

              
                await context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER Infarny_WeaponTotalCost ON Infarny_Weapon");

       
                txtName.Clear();
                txtCount.Clear();
                txtUnitCost.Clear();

       
            }




        }

        private void bt_InfantryClick(object sender, RoutedEventArgs e)
        {
            var win_infantry = new WinInfarny();
            NavigatorObject.Switch(win_infantry);
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


        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem selectedItem)
            {
          
                txtName.Text = selectedItem.Header.ToString();
            }
        }

        private void CreateQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-N5K3CGS\\SQLEXPRESS01;Initial Catalog=MilitaryExport;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    connection.Open();

                   
                    int Infarny_weaponId;
                    int Infantry_equipmentId;
          

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Infarny_Weapon ORDER BY Id DESC", connection))
                    {
                        Infarny_weaponId = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Infantry_equipment ORDER BY Id DESC", connection))
                    {
                        Infantry_equipmentId = (int)cmd.ExecuteScalar();
                    }

                   
                   
                    string insertQuery = "INSERT INTO Ground_forces_request (Infarny_WeaponId, Infantry_equipmentId, Cost) " +
                                         "VALUES (@Infarny_WeaponId, @Infantry_equipmentId, 0)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Infarny_WeaponId", Infarny_weaponId);
                        cmd.Parameters.AddWithValue("@Infantry_equipmentId", Infantry_equipmentId);

                        // Выполните SQL-запрос
                        cmd.ExecuteNonQuery();
                        string triggerQuery = "UPDATE Ground_forces_request SET Cost = 0 WHERE Id = SCOPE_IDENTITY()"; // Используйте SCOPE_IDENTITY() для получения ID только что вставленной записи
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
