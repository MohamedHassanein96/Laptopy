using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LaptopyCore.Model
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "the Length must be greater than 2")]
        [MaxLength(30, ErrorMessage = "the Length mustn't be greater than 30")]
        public string Name { get; set; } = string.Empty;


        [JsonIgnore]
        public IEnumerable<Product> Products { get; set; } = new HashSet<Product>();
    }
}
