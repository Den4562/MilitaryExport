using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
using WpfAppMilitaryExport.DataBase.Table;
using WpfAppMilitaryExport.DB;
using WpfAppMilitaryExport.Navigator;
namespace WpfAppMilitaryExport.Icons
{
    /// <summary>
    /// Логика взаимодействия для WinInfarny.xaml
    /// </summary>
    public partial class WinInfarny : UserControl
    {
        public WinInfarny()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MilitaryDBContext())
            {
                // Отключаем триггер
                await context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER Infantry_equipmentTotalCost ON Infantry_equipment");

                // Создаем новый объект Airplane на основе введенных данных
                var newInfantry = new Infantry_equipment()
                {
                    Name = txtName.Text,
                    Count = int.Parse(txtCount.Text),
                    Unit_Cost = decimal.Parse(txtUnitCost.Text),
                    Total_Cost = int.Parse(txtCount.Text) * decimal.Parse(txtUnitCost.Text)
                };

                // Добавляем новый самолет в контекст и сохраняем изменения в базе данных
                context.Infantry_equipment.Add(newInfantry);
                context.SaveChanges();

                // Включаем триггер обратно
                await context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER Infantry_equipmentTotalCost ON Infantry_equipment");

                // Очищаем поля ввода
                txtName.Clear();
                txtCount.Clear();
                txtUnitCost.Clear();

                // Обновляем отображение списка самолетов или выполните другие необходимые действия
            }




        }

        private void bt_WeaponClick(object sender, RoutedEventArgs e)
        {
            var win_weapon = new WinWeapon();
            NavigatorObject.Switch(win_weapon);
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
                // Получите текст выбранного элемента и установите его в поле txtName
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
