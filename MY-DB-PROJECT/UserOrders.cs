using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY_DB_PROJECT
{
    public static class UserOrders
    {
        public static List<Order> Orders = new List<Order>();
    }

    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
    }

}
