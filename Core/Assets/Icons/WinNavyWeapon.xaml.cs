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
    /// Логика взаимодействия для WinNavyWeapom.xaml
    /// </summary>
    public partial class WinNavyWeapon : UserControl
    {
        public WinNavyWeapon()
        {
            InitializeComponent();
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MilitaryDBContext())
            {
  
                await context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER UpdateWeaponTotalCost ON Navy_Weapon");

     
                var newNavyWeapon = new Navy_Weapon()
                {
                    Name = txtName.Text,
                    Count = int.Parse(txtCount.Text),
                    Unit_Cost = decimal.Parse(txtUnitCost.Text),
                    Total_Cost = int.Parse(txtCount.Text) * decimal.Parse(txtUnitCost.Text)
                };

          
                context.Navy_Weapon.Add(newNavyWeapon);
                context.SaveChanges();

     
                await context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER UpdateWeaponTotalCost ON Navy_Weapon");
   
                txtName.Clear();
                txtCount.Clear();
                txtUnitCost.Clear();

      
            }




        }

        private void bt_DetailsClick(object sender, RoutedEventArgs e)
        {
            var win_navyDetails = new WinNavyDetails();
            NavigatorObject.Switch(win_navyDetails) ;
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

        private void CreateQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-N5K3CGS\\SQLEXPRESS01;Initial Catalog=MilitaryExport;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    connection.Open();


                    int Navy_WeaponId;
                    int Navy_DetailsId;


                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Navy_Weapon ORDER BY Id DESC", connection))
                    {
                        Navy_WeaponId = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Navy_Details ORDER BY Id DESC", connection))
                    {
                        Navy_DetailsId = (int)cmd.ExecuteScalar();
                    }



                    string insertQuery = "INSERT INTO Navy_forces_request (Navy_WeaponId, Navy_DetailsId, Cost) " +
                                         "VALUES (@Navy_WeaponId, @Navy_DetailsId, 0)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Navy_WeaponId", Navy_WeaponId);
                        cmd.Parameters.AddWithValue("@Navy_DetailsId", Navy_DetailsId);

                        // Выполните SQL-запрос
                        cmd.ExecuteNonQuery();
                        string triggerQuery = "UPDATE Navy_forces_request SET Cost = 0 WHERE Id = SCOPE_IDENTITY()"; // Используйте SCOPE_IDENTITY() для получения ID только что вставленной записи
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
