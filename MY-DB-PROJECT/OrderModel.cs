using System;
using System.Collections.Generic;

namespace MY_DB_PROJECT
{
    public class Class1
    {
        public string OrderID { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public string UserID { get; set; }
    }

    public static class SampleOrders
    {
        public static List<Class1> AllOrders = new List<Class1>()
        {
            new Class1{ OrderID="1001", Product="Apple", Quantity=3, Price=300, DateTime=DateTime.Parse("2025-12-01 16:30"), UserID="U1"},
            new Class1{ OrderID="1002", Product="Milk", Quantity=1, Price=180, DateTime=DateTime.Parse("2025-12-02 14:20"), UserID="U2"},
            new Class1{ OrderID="1003", Product="Juice", Quantity=2, Price=240, DateTime=DateTime.Parse("2025-12-03 13:15"), UserID="U1"},
            new Class1{ OrderID="1004", Product="Rice 5Kg", Quantity=1, Price=1400, DateTime=DateTime.Parse("2025-12-04 18:10"), UserID="U3"},
            new Class1{ OrderID="1005", Product="Banana", Quantity=12, Price=300, DateTime=DateTime.Parse("2025-12-05 15:45"), UserID="U1"},
        };
    }
}
