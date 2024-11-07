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

       

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _mapper.Map<IEnumerable<CategoryDTO>>(_unitOfWorkRepository.Categories);

            if (categories!=null)
            {
                return Ok(categories);

            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAll(int id)
        {
            var category = _mapper.Map<CategoryDTO>(_unitOfWorkRepository.Categories.Get(c=>c.Id==id).FirstOrDefault());
            if (category!=null)
            {
                return Ok(category);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Create(CategoryDTO  categoryDTO)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryDTO);
                _unitOfWorkRepository.Categories.Create(category);
                _unitOfWorkRepository.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
