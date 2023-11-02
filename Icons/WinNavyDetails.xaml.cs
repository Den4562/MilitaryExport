﻿using Microsoft.Data.SqlClient;
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
    /// Логика взаимодействия для WinNavyDetails.xaml
    /// </summary>
    public partial class WinNavyDetails : UserControl
    {
        public WinNavyDetails()
        {
            InitializeComponent();
        }

        private void bt_WeapomClick(object sender, RoutedEventArgs e)
        {
            var win_NavyWeapon = new WinNavyWeapon();
            NavigatorObject.Switch(win_NavyWeapon);
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

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MilitaryDBContext())
            {
                // Отключаем триггер
                await context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER  UpdateDetailTotalCost ON Navy_Details");

                // Создаем новый объект Airplane на основе введенных данных
                var newDetails = new Navy_Details()
                {
                    Name = txtName.Text,
                    Count = int.Parse(txtCount.Text),
                    Unit_Cost = decimal.Parse(txtUnitCost.Text),
                    Total_Cost = int.Parse(txtCount.Text) * decimal.Parse(txtUnitCost.Text)
                };

                // Добавляем новый самолет в контекст и сохраняем изменения в базе данных
                context.Navy_Details.Add(newDetails);
                context.SaveChanges();

                // Включаем триггер обратно
                await context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER  UpdateDetailTotalCost ON Navy_Details");

                // Очищаем поля ввода
                txtName.Clear();
                txtCount.Clear();
                txtUnitCost.Clear();

                // Обновляем отображение списка самолетов или выполните другие необходимые действия
            }




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
