﻿using AutoMapper;
using LaptopyCore.IUnitOfWorkRepository;
using LaptopyCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public IActionResult Index(string? query = null)
        {
            var newArrivals = _unitOfWorkRepository.Products.Get(p => p.IsNewArrival);
            var trendingProducts = _unitOfWorkRepository.Products.Get(p => p.IsTrending);
            var specialProducts = _unitOfWorkRepository.Products.Get(p => p.IsSpecial );

            var result = new
            {
                NewArrivals = newArrivals,
                TrendingProducts = trendingProducts,
                SpecialProducts = specialProducts
            };

            IQueryable<Product> products = _unitOfWorkRepository.Products.Get(null, query => query.Include(p => p.ProductImages)).AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(m => m.Name.ToLower().Contains(query.Trim().ToLower()));
                return Ok(products);
            }
            else
            {
                return Ok(result);

            }

        }

        [HttpGet("Details")]
        public IActionResult Details(int productId)
        {
            var product = _unitOfWorkRepository.Products.Get(p => p.Id == productId, query => query.Include(p => p.ProductImages)).FirstOrDefault();
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpGet("GetByModel")]
        public IActionResult GetByModel(string? model = null)
        {
            var products = _unitOfWorkRepository.Products.Get(null, query => query.Include(p => p.ProductImages)).ToList();

            if (model != null)
            {
                var filterModel = _unitOfWorkRepository.Products.Get(e => e.Model.Contains(model.Trim()));
                return Ok(filterModel);
            }
            else
            {
                return Ok(products);
            }

        }

        [HttpGet("GetByPrice")]
        public IActionResult SearchByPriceRange(int minPrice = 0, int maxPrice = 0)
        {
            if (minPrice == 0 && maxPrice == 0)
            {
                var allProducts = _unitOfWorkRepository.Products.Get().ToList();
                return Ok(allProducts);
            }
            else
            {
                var products = _unitOfWorkRepository.Products.GetByPriceRange(
                minPrice: minPrice,
                maxPrice: maxPrice,
                include: query => query.Include(p => p.ProductImages));
                return Ok(products);
            }

        }
        [HttpGet("GetByRating")]
        public IActionResult GetByRating(int rate)
        {
            var products = _unitOfWorkRepository.Products.Get().ToList();
            if (rate == 0)
            {
                return Ok(products);

            }
            else
            {
                var productsByRating = _unitOfWorkRepository.Products.Get(p => p.Rating == rate).ToList();
                return Ok(productsByRating);
            }
        }
    }
}
