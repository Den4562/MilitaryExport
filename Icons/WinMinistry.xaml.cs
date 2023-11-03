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




        private void LoadNavy()
        {
            using (var context = new MilitaryDBContext())
            {
                var result = context.Navy_forces_request
                    .Include(nfr => nfr.Navy_Weapon)
                    .Include(nfr => nfr.Navy_Details)
                    .Select(nfr => new
                    {
                        RequestId = nfr.Id,
                        WeaponName = nfr.Navy_Weapon.Name,
                        WeaponCount = nfr.Navy_Weapon.Count,
                        WeaponUnitCost = nfr.Navy_Weapon.Unit_Cost,
                        WeaponTotalCost = nfr.Navy_Weapon.Total_Cost,
                        DetailsName = nfr.Navy_Details.Name,
                        DetailsCount = nfr.Navy_Details.Count,
                        DetailsUnitCost = nfr.Navy_Details.Unit_Cost,
                        DetailsTotalCost = nfr.Navy_Details.Total_Cost,
                        RequestCost = nfr.Cost
                    })
                    .ToList();

                DataTable.ItemsSource = result;
            }
        }

        private void LoadGround()
        {
            using (var context = new MilitaryDBContext())
            {
                var result = context.Ground_forces_request
                    .Include(gfr => gfr.Infarny_Weapon)
                    .Include(gfr => gfr.Infantry_equipment)
                    .Select(gfr => new
                    {
                        RequestId = gfr.Id,
                        WeaponName = gfr.Infarny_Weapon.Name,
                        WeaponCount = gfr.Infarny_Weapon.Count,
                        WeaponUnitCost = gfr.Infarny_Weapon.Unit_Cost,
                        WeaponTotalCost = gfr.Infarny_Weapon.Total_Cost,
                        EquipmentName = gfr.Infantry_equipment.Name,
                        EquipmentCount = gfr.Infantry_equipment.Count,
                        EquipmentUnitCost = gfr.Infantry_equipment.Unit_Cost,
                        EquipmentTotalCost = gfr.Infantry_equipment.Total_Cost,
                        RequestCost = gfr.Cost
                    })
                    .ToList();

                DataTable.ItemsSource = result;
            }
        }

        private void LoadMinistry()
        {
            using (var context = new MilitaryDBContext())
            {
                var result = context.Order_Ministry_of_Defence
                    .Include(omod => omod.Army_Order) 
                    .Select(omod => new
                    {
                        RequestId = omod.Id,
                        ArmyOrderName = omod.Army_OrderID, // Замените Army_Order.Name на соответствующее поле
                        StartDate = omod.StartDate
                    })
                    .ToList();

                DataTable2.ItemsSource = result;
            }
        }


        private void LoadAir()
        {
            using (var context = new MilitaryDBContext())
            {
                var result = context.Air_forces_request
                    .Include(afr => afr.Airplane)
                    .Include(afr => afr.Ammo)
                    .Include(afr => afr.Details)
                    .Select(afr => new
                    {
                        RequestId = afr.Id,
                        AirplaneName = afr.Airplane.Name,
                        AirplaneCount = afr.Airplane.Count,
                        AirplaneUnitCost = afr.Airplane.Unit_Cost,
                        AirplaneTotalCost = afr.Airplane.Total_Cost,
                        AmmoName = afr.Ammo.Name,
                        AmmoCount = afr.Ammo.Count,
                        AmmoUnitCost = afr.Ammo.Unit_Cost,
                        AmmoTotalCost = afr.Ammo.Total_Cost,
                        DetailsName = afr.Details.Name,
                        DetailsCount = afr.Details.Count,
                        DetailsUnitCost = afr.Details.Unit_Cost,
                        DetailsTotalCost = afr.Details.Total_Cost,
                        RequestCost = afr.Cost
                    })
                    .ToList();

                DataTable.ItemsSource = result;
            }
        }


        private void CreateQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-N5K3CGS\\SQLEXPRESS01;Initial Catalog=MilitaryExport;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    connection.Open();


                    int Army_OrderId;
                  


                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Army_Order ORDER BY Id DESC", connection))
                    {
                        Army_OrderId = (int)cmd.ExecuteScalar();
                    }
                    string insertQuery = "INSERT INTO Order_Ministry_of_Defence (Army_OrderId, StartDate) " +
                                         "VALUES (@Army_OrderId, 0)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Army_OrderId", Army_OrderId);

                        // Выполните SQL-запрос
                        cmd.ExecuteNonQuery();
                        string triggerQuery = "UPDATE Order_Ministry_of_Defence SET StartDate = 0 WHERE Id = SCOPE_IDENTITY()"; // Используйте SCOPE_IDENTITY() для получения ID только что вставленной записи
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

        private void bt_showNavy(object sender, RoutedEventArgs e)
        {
            LoadNavy();
        }

        private void bt_showMinistry(object sender, RoutedEventArgs e)
        {
            LoadMinistry();
        }

        private void bt_showAir(object sender, RoutedEventArgs e)
        {
            LoadAir();
        }

        private void click_Main(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            NavigatorObject.Switch(main);
        }

        private void bt_showGround(object sender, RoutedEventArgs e)
        {
            LoadGround();
        }

        private void click_Exit(object sender, RoutedEventArgs e)
        {
            var win_Autorized = new Autorizeds();
            NavigatorObject.Switch(win_Autorized);
        }
    }
}
