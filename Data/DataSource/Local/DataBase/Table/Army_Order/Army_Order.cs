using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMilitaryExport.DataBase.Table
{
  public  class Army_Order
    {
        public int Id { get; set; }
        public int Ground_forces_requestId { get; set; }

        public int Air_forces_requestId { get; set; }

        public int Navy_forces_requestId { get; set; }

        public Army_Order()
        { }


        public Army_Order(int id, int ground_forces_requestId, int Air_forces_requestId, int Navy_forces_requestId)
        {
             Id = id;
            Ground_forces_requestId = ground_forces_requestId;
            Air_forces_requestId = Air_forces_requestId;
            Navy_forces_requestId = Navy_forces_requestId;
        }
    }
}
