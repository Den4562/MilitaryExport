using System;

namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Air_forces_request
    {
        public int Id { get; set; }
        public int AirplaneId { get; set; }
        public int AmmoId { get; set; }
        public int DetailsId { get; set; }
        public decimal Cost { get; set; }

        public Air_forces_request()
        {
        }

        public Airplane Airplane { get; set; } // Навигационное свойство на Navy_Weapon
        public Ammo Ammo { get; set; } //
        public Details Details { get; set; } //


        public Air_forces_request(int id, int airplaneId, int ammoId, int detailsId, decimal cost)
        {
            Id = id;
            AirplaneId = airplaneId;
            AmmoId = ammoId;
            DetailsId = detailsId;
            Cost = cost;
        }
    }
}