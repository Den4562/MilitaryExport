using System;

namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Ground_forces_request
    {
        public int Id { get; set; }
        public int Infarny_WeaponId { get; set; }
        public int Infantry_equipmentId { get; set; }
        public decimal Cost { get; set; }

        public Ground_forces_request()
        {
        }

        public Infantry_equipment Infantry_equipment { get; set; } // Навигационное свойство на Navy_Weapon
        public Infarny_Weapon Infarny_Weapon { get; set; } //

        public Ground_forces_request(int id, int infarny_WeaponId, int infantry_equipmentId, decimal cost)
        {
            Id = id;
            Infarny_WeaponId = infarny_WeaponId;
            Infantry_equipmentId = infantry_equipmentId;
            Cost = cost;
        }
    }
}