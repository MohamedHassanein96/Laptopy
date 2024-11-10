using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        public Decimal Rating { get; set; }
        public int CategoryID { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public Category Category { get; set; } = null!;
        public bool IsNewArrival { get; set; }
        public bool IsTrending { get; set; }
        public bool IsSpecial { get; set; }
    }
}
