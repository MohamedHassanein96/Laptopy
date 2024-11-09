using AutoMapper;
using LaptopyCore.IUnitOfWorkRepository;
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
        public IActionResult Index()
        {

            var products = _unitOfWorkRepository.Products.Get(null, query => query.Include(p => p.ProductImages)).ToList();
            if (products != null)
            {
                return Ok(products);
            }
            else
            {
                return NotFound();
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
