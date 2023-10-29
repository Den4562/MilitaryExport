using System;

namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Production
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
        public string Region { get; set; }
        public int OrderId { get; set; }

        public Production()
        {
        }

        public Production(int id, string name, bool state, string region, int orderId)
        {
            Id = id;
            Name = name;
            State = state;
            Region = region;
            OrderId = orderId;
        }
    }
}