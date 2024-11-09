using LaptopyCore.IRepository.IProductRepository;
using LaptopyCore.Model;
using LaptopyEF.Data;
using LaptopyEF.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyEF.Repository.ProductRepository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetByPriceRange(int minPrice = 0, int maxPrice = 0, Func<IQueryable<Product>, IQueryable<Product>> include = null) 
        {
            IQueryable<Product> query = _context.Set<Product>();
            if (include != null)
            {
                query = include(query);
            }
             query = query.Where(item =>
              ( item.Price >= minPrice) &&
              ( item.Price <= maxPrice)
           );

            return query.ToList();

        }
    }
}
