using LaptopyCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public List<IFormFile> Images { get; set; } 
        public string Model { get; set; }
        public Decimal Rating { get; set; }

        public int CategoryID { get; set; }
    }
}
