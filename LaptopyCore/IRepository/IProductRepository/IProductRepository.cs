using LaptopyCore.IRepository.IBaseRepository;
using LaptopyCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyCore.IRepository.IProductRepository
{
    public interface IProductRepository:IBaseRepository<Product>
    {
        public IEnumerable<Product> GetByPriceRange(int minPrice = 0, int maxPrice = 0, Func<IQueryable<Product>, IQueryable<Product>> include = null);


    }
}
