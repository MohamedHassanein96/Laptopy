using AutoMapper;
using LaptopyCore.DTO;
using LaptopyCore.IUnitOfWorkRepository;
using LaptopyCore.Model;
using LaptopyEF.UnitOfWorkRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

       

        [HttpGet("GetAll")]
        public IActionResult Index()
        {
          
            var categories = _unitOfWorkRepository.Categories.Get().ToList();
            if (categories != null)
            {
                return Ok(categories);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("Details")]
        public IActionResult Details(int id)
        {
            //var category = _mapper.Map<CategoryDTO>(_unitOfWorkRepository.Categories.Get(c=>c.Id==id).FirstOrDefault());
            //if (category!=null)
            //{
            //    return Ok(category);
            //}
            //else
            //{
            //    return NotFound();
            //}

            var category = _unitOfWorkRepository.Categories.Get(c => c.Id == id).FirstOrDefault();
            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost("Create")]
        public IActionResult Create(Category  category)
        {
            if (ModelState.IsValid)
            {
                //var category = _mapper.Map<Category>(categoryDTO
                _unitOfWorkRepository.Categories.Create(category);
                _unitOfWorkRepository.SaveChanges();
                return Created($"{Request.Scheme}://{Request.Host}/api/Category/Details?categoryId={category.Id}", category);
            }
            return BadRequest();
        }

        [HttpPut("Edit")]
        public IActionResult Edit(Category category)
        {
            var existingCategory = _unitOfWorkRepository.Categories.Get(c => c.Id == category.Id,null,false).FirstOrDefault();

            if (existingCategory != null)
            {
                if (ModelState.IsValid)
                {
                    _unitOfWorkRepository.Categories.Edit(category);
                    _unitOfWorkRepository.SaveChanges();
                    return Created($"{Request.Scheme}://{Request.Host}/api/Category/Details?categoryId={category.Id}", category);
                }
                return BadRequest();
            }
            else
            {
                return NotFound();

            }


        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int categoryId)
        {
            var category = _unitOfWorkRepository.Categories.Get(c => c.Id == categoryId).FirstOrDefault();
            if (category!= null)
            {
                _unitOfWorkRepository.Categories.Delete(category);
                _unitOfWorkRepository.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

    }

}
