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


   
    }
}
