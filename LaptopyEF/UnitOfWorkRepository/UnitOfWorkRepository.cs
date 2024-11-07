using LaptopyCore.IRepository.IBaseRepository;
using LaptopyCore.IUnitOfWorkRepository;
using LaptopyCore.Model;
using LaptopyEF.Data;
using LaptopyEF.Repository.BaseRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LaptopyEF.UnitOfWorkRepository
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        public IBaseRepository<Product> Products { get; private set; }

        public IBaseRepository<ProductImages> ProductImages { get; private set; }

        public IBaseRepository<Category> Categories { get; private set; }

        public IBaseRepository<ContactUs> ContactUs { get; private set; }

        public IBaseRepository<Cart> Carts { get; private set; }

        private readonly ApplicationDbContext _context;
        public UnitOfWorkRepository(ApplicationDbContext context)
        {
            _context = context;
            InitializeRepositories();
        }
        private void InitializeRepositories()
        {
            Products = CreateRepository<Product>();
            ProductImages = CreateRepository<ProductImages>();
            Categories = CreateRepository<Category>();
            ContactUs = CreateRepository<ContactUs>();
            Carts = CreateRepository<Cart>();
        }

        private IBaseRepository<T> CreateRepository<T>() where T : class
        {
            return new BaseRepository<T> (_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
