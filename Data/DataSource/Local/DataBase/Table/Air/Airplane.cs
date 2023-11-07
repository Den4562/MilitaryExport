using System;


namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Airplane
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public decimal Unit_Cost { get; set; }

        public decimal Total_Cost { get; set; }

        public Airplane()
        {
        }

        public Airplane(int id, string name, int count, decimal unit_cost, decimal total_cost)
        {
            Id = id;
            Name= name;
            Count = count;
            Unit_Cost = unit_cost;
            Total_Cost = total_cost;
        }
    }
}
