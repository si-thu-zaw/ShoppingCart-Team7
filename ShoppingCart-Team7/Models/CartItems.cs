using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart_Team7.Models
{
    public class CartItems
    {
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string User { get; set; }
    }
}
