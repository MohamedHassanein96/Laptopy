using LaptopyCore.IRepository.IBaseRepository;
using LaptopyCore.IRepository.IProductRepository;
using LaptopyCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.IUnitOfWorkRepository
{
    public interface IUnitOfWorkRepository :IDisposable
    {
        //IBaseRepository<Product> Products { get; }
        IBaseRepository<ProductImages> ProductImages { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<ContactUs> ContactUs { get; }
        IBaseRepository<Cart> Carts { get; }
        IProductRepository Products { get; }

        void SaveChanges();
    }

}
