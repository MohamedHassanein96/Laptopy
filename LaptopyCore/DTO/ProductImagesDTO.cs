using LaptopyCore.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.DTO
{
    public class ProductImagesDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public int ProductId { get; set; }

        [ValidateNever]
        public Product Product { get; set; }
    }
}
