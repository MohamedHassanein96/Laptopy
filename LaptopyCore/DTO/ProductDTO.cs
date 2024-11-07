using LaptopyCore.Model;
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

        [Required]
        [MinLength(3, ErrorMessage = "the Length must be greater than 2")]
        [MaxLength(30, ErrorMessage = "the Length mustn't be greater than 30")]
        public string Name { get; set; } = string.Empty;
        [Range(50, 1000, ErrorMessage = "the Range must be between than 50 to 1000")]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal Discount { get; set; }
        [Range(1, 1000, ErrorMessage = "Quantity must be between 0 and 100.")]
        public int Quantity { get; set; } = 1000;
        [Required]
        [MinLength(3, ErrorMessage = "the Length must be greater than 2")]
        [MaxLength(100, ErrorMessage = "the Length mustn't be greater than 100")]
        public string Description { get; set; } = string.Empty;

        [ValidateNever]
        public List<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
        [Required]
        [MinLength(3, ErrorMessage = "the Length must be greater than 2")]
        [MaxLength(30, ErrorMessage = "the Length mustn't be greater than 30")]
        public string Model { get; set; } = string.Empty;
        public int CategoryID { get; set; }
        [ValidateNever]

        public Category Category { get; set; } = null!;
    }
}
