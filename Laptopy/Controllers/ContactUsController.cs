using LaptopyCore.DTO;
using LaptopyCore.IUnitOfWorkRepository;
using LaptopyCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ContactUsController(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }
        [HttpPost("Create")]
        public IActionResult Create(ContactUs contactUs)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _unitOfWorkRepository.ContactUs.Create(contactUs);
                _unitOfWorkRepository.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }
        [HttpGet("GetAll")]
        public IActionResult Index()

        {
            var contactUs = _unitOfWorkRepository.ContactUs.Get();
            return Ok(contactUs);
        }
    }
}
