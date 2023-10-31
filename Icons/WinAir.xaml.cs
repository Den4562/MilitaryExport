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

namespace WpfAppMilitaryExport.Icons
{
    /// <summary>
    /// Логика взаимодействия для WinAir.xaml
    /// </summary>
    public partial class WinAir : UserControl
    {
        public WinAir()
        {
            InitializeComponent();


        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MilitaryDBContext())
            {
                // Отключаем триггер
                await context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER UpdateAirplaneTotalCost ON Airplane");

                // Создаем новый объект Airplane на основе введенных данных
                var newAirplane = new Airplane
                {
                    Name = txtName.Text,
                    Count = int.Parse(txtCount.Text),
                    Unit_Cost = decimal.Parse(txtUnitCost.Text),
                    Total_Cost = int.Parse(txtCount.Text) * decimal.Parse(txtUnitCost.Text)
                };

                // Добавляем новый самолет в контекст и сохраняем изменения в базе данных
                context.Airplane.Add(newAirplane);
                context.SaveChanges();

                // Включаем триггер обратно
                await context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER UpdateAirplaneTotalCost ON Airplane");

                // Очищаем поля ввода
                txtName.Clear();
                txtCount.Clear();
                txtUnitCost.Clear();

                // Обновляем отображение списка самолетов или выполните другие необходимые действия
            }
        }
    }
}
