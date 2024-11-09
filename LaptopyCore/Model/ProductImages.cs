using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LaptopyCore.Model
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        [JsonIgnore]
        [ValidateNever]
        public Product Product { get; set; }
    }
}

