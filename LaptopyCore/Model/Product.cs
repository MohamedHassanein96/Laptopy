using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; } 
        public List<ProductImages> ProductImages { get; set; } 
        public string Model { get; set; } 
        public int CategoryID { get; set; }
        public Category Category { get; set; } = null!;
    }
}
