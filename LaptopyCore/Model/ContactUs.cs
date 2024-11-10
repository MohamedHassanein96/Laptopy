using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.Model
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "the Length must be greater than 2")]
        [MaxLength(30, ErrorMessage = "the Length mustn't be greater than 30")]
        public string Name { get; set; }
        [MinLength(3, ErrorMessage = "the Length must be greater than 2")]
        [MaxLength(30, ErrorMessage = "the Length mustn't be greater than 30")]

        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "the Length must be greater than 10")]
        [MaxLength(1000, ErrorMessage = "the Length mustn't be greater than 1000")]
        public string Message { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "the Length must be greater than 3")]
        [MaxLength(20, ErrorMessage = "the Length mustn't be greater than 20")]
        public string Subject { get; set; }
        public bool Status { get; set; } = false;
    }
}
