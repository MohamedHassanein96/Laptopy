using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LaptopyCore.DTO;
using LaptopyCore.Model;
namespace LaptopyCore.Utility
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        
        {
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<ContactUs, ContactUsDTO>().ReverseMap();
            CreateMap<ProductImages, ProductImagesDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();

        }
    }
}
