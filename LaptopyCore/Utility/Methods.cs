using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.Utility
{
    public class Methods
    {
        public static List<string> UploadImages(List<IFormFile> images)
        {
            var imageFileNames = new List<string>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        image.CopyTo(stream);
                    }

                    imageFileNames.Add(fileName); 
                }
            }

            return imageFileNames;
        }
    }
}
