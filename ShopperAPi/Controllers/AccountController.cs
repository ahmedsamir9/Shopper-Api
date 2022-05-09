using Core.Entities.Identity;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.DTOS;

namespace ShopperAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerUser)
        {
            if (CheckEmailExists(registerUser.Email).Result.Value)
            {
                ModelState.AddModelError("Email", "Email is already exists!");
                return BadRequest(ModelState);
            }

            if (CheckUserNameExists(registerUser.UserName).Result.Value)
            {
                ModelState.AddModelError("UserName", "User Name is already exists!");
                return BadRequest(ModelState);
            }

            var user = new AppUser()
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                FullName = registerUser.FullName,
                PhoneNumber = registerUser.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!ValidIdentityResult(result))
                return BadRequest(ModelState);

            result = await _userManager.AddToRoleAsync(user, RolesConstantHelper.ClientRole);

            if (!ValidIdentityResult(result))
                return BadRequest(ModelState);

            return Created("", await _tokenService.CreateToken(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.User);

            if (user is null)
                user = await _userManager.FindByNameAsync(loginUser.User);

            if (user is null)
            {
                ModelState.AddModelError("Email-UserName", "Email or User Name are not exists!");
                return BadRequest(ModelState);
            }

            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
                return BadRequest("Login Refused: Invalid User Name or Password");


            return Ok(await _tokenService.CreateToken(user));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email) =>
            await _userManager.FindByEmailAsync(email) is not null;

        [HttpGet("usernameexists")]
        public async Task<ActionResult<bool>> CheckUserNameExists(string userName) =>
            await _userManager.FindByNameAsync(userName) is not null;

        private bool ValidIdentityResult(IdentityResult result)
        {
            if (result.Succeeded)
                return true;

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return false;
        }
    }
}
