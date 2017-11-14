
using System;

namespace Common.Views
{
    public class VwShoppingCart
    {
        public decimal Tax { get; set; }
        public Guid ID { get; set; }
        public string Image { get; set; }
        public string Username { get; set; }
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}
