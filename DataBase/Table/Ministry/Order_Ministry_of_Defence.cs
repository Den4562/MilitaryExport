using System;

namespace WpfAppMilitaryExport.DataBase.Table
{
    public class Order_Ministry_of_Defence
    {
        public int Id { get; set; }
        public int Army_OrderID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Order_Ministry_of_Defence()
        {
        }

        public Order_Ministry_of_Defence(int id, int armyOrderId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Army_OrderID = armyOrderId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}