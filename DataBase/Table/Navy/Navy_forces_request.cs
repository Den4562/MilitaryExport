using System;

namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Navy_forces_request
    {
        public int Id { get; set; }
        public int Navy_WeaponId { get; set; }
        public int Navy_DetailsId { get; set; }
        public decimal Cost { get; set; }

        public Navy_forces_request()
        {
        }

        public Navy_Weapon Navy_Weapon { get; set; } // Навигационное свойство на Navy_Weapon
        public Navy_Details Navy_Details { get; set; } //

        public Navy_forces_request(int id, int navy_WeaponId, int navy_DetailsId, decimal cost)
        {
            Id = id;
            Navy_WeaponId = navy_WeaponId;
            Navy_DetailsId = navy_DetailsId;
            Cost = cost;
        }
    }
}