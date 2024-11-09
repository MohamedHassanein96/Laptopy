using AutoMapper;
using LaptopyCore.DTO;
using LaptopyCore.IUnitOfWorkRepository;
using LaptopyCore.Model;
using LaptopyCore.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;




        public ProductController(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
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
        [HttpPost("Create")]
        public IActionResult Create(ProductDTO productDTO)
        {

            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(productDTO);
                var imageFileNames = Methods.UploadImages(productDTO.Images);

                product.ProductImages = imageFileNames.Select
                    (fileName => new ProductImages
                    {
                        ImageUrl = fileName,
                        Product = product
                    }).ToList();

                _unitOfWorkRepository.Products.Create(product);
                _unitOfWorkRepository.SaveChanges();
                return Ok("Product and images saved successfully.");
            }
            else
            {
                return BadRequest();
            }



        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int productId)
        {
            var product = _unitOfWorkRepository.Products.Get(p => p.Id == productId).FirstOrDefault();
            if (product != null)
            {
                _unitOfWorkRepository.Products.Delete(product);
                _unitOfWorkRepository.SaveChanges();
                return Ok();
            }
            return NotFound();
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


        [HttpPut("Edit/{id}")]
        public  IActionResult Edit(int id, [FromForm] ProductDTO productDTO)
        {
            ModelState.Remove("Images");
            var existingProduct = _unitOfWorkRepository.Products.Get(p => p.Id == id, null, false).FirstOrDefault();

            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            var product = _mapper.Map<Product>(productDTO);

            if (productDTO.Images != null && productDTO.Images.Any())
            {
                var uploadedImageFileNames = Methods.UploadImages(productDTO.Images);

                product.ProductImages = uploadedImageFileNames.Select(fileName => new ProductImages { ImageUrl = $"/images/{fileName}" }).ToList();
            }
            else
            {
                product.ProductImages = existingProduct.ProductImages;
            }

            _unitOfWorkRepository.Products.Edit(product);
            _unitOfWorkRepository.SaveChanges();

            return Ok(product);
        }


        [HttpGet("GetByModel")]
        public IActionResult GetByModel(string? model=null)
        {
            var products = _unitOfWorkRepository.Products.Get(null, query => query.Include(p => p.ProductImages)).ToList();

            if (model!=null)
            {
               var filterModel =_unitOfWorkRepository.Products.Get(e => e.Model.Contains(model.Trim()));
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
            if (minPrice==0 && maxPrice ==0)
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
               var productsByRating =_unitOfWorkRepository.Products.Get(p=>p.Rating==rate).ToList();
                return Ok(productsByRating);
            }
        }
    }
}
