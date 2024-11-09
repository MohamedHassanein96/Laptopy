using AutoMapper;
using LaptopyCore.DTO;
using LaptopyCore.Model;
using LaptopyCore.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManager<ApplicationUser> _UserManager;
        public SignInManager<ApplicationUser> _SignInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager ,RoleManager<IdentityRole> roleManager,SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _UserManager = userManager;
            _roleManager = roleManager;
            _SignInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("Register")]
       public async Task <IActionResult> Register(ApplicationUserDTO applicationUserDTO)
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await  _roleManager.CreateAsync(new(SD.adminRole));
                await _roleManager.CreateAsync(new(SD.customerRole));
                await _roleManager.CreateAsync(new(SD.companyRole));
            }
            if (ModelState.IsValid)
            {
                var applicationUser = _mapper.Map<ApplicationUser>(applicationUserDTO);
                applicationUser.UserName = applicationUserDTO.Email;
                var result =  await  _UserManager.CreateAsync(applicationUser, applicationUserDTO.Password);
                if (result.Succeeded)
                {
                    await _SignInManager.SignInAsync(applicationUser, false);
                    await _UserManager.AddToRoleAsync(applicationUser, SD.customerRole);
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO  loginDTO)
        {
              var user = await _UserManager.Users
                .FirstOrDefaultAsync(u => u.FirstName == loginDTO.FirstName && u.LastName == loginDTO.LastName);

            if (user!=null)
            {
                var result = await _UserManager.CheckPasswordAsync(user, loginDTO.Password);
                if (result)
                {
                    await _SignInManager.SignInAsync(user, loginDTO.RememberMe);
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "there are errors");
                    return BadRequest();
                }

            }
            return NotFound();
        }

        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
            _SignInManager.SignOutAsync();
            return Ok();

        }
    }
}
