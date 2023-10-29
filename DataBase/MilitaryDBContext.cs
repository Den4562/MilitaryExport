using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMilitaryExport.DataBase.Table;
using WpfAppMilitaryExport.DataBase.Table.Army_Order;

namespace WpfAppMilitaryExport.DB
{
    class MilitaryDBContext: DbContext
    {
        public DbSet<Air_forces_request> Air_forces_request { get; set; }
        public DbSet<Airplane> Airplane { get; set; }
        public DbSet<Ammo> Ammo { get; set; }
        public DbSet<Details> Details { get; set; }
        public DbSet<Army_Order> Army_order  { get; set; }
        public DbSet<Ground_forces_request> Ground_forces_request { get; set; }
        public DbSet<Infantry_equipment> Infantry_equipment { get; set; }
        public DbSet<Infarny_Weapon> Infarny_Weapon { get; set; }
        public DbSet<Order_Ministry_of_Defence> Order_Ministry_of_Defence { get; set; }
        public DbSet<Production> Production { get; set; }
        public DbSet<Account_Command> Account_Command { get; set; }
        public DbSet<Account_Ministry> Account_Ministry { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-N5K3CGS\\SQLEXPRESS01;Initial Catalog=MilitaryExport;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
