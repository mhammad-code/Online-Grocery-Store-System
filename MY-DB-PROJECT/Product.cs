using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY_DB_PROJECT
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Quantity { get; set; }
        public int AvailableStock { get; set; }
    }

}
