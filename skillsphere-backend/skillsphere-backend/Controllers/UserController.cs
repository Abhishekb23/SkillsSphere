using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Services;

namespace skillsphere_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST: api/User/Admin/Registration
        [HttpPost("Admin/Registration")]
        public async Task<ActionResult<UserDto>> AddAdmin([FromBody] CreateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdUser = await _userService.CreateUserAsync(dto);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/User/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _userService.LoginAsync(dto.Email, dto.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        // POST: api/User/Registration
        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            try
            {
                await _userService.SendRegistrationOtpAsync(dto);
                return Ok(new { Message = "OTP sent to your email. Please verify within 5 minutes." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/User/verify-otp
        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            try
            {
                var user = await _userService.VerifyOtpAndRegisterAsync(dto.Email, dto.OtpCode);
                return Ok(new { Message = "Registration successful!", User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ✅ GET: api/User/profile/{userId}
        [HttpGet("profile/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var profile = await _userService.GetProfileAsync(userId);
            if (profile == null)
                return NotFound(new { Message = "Profile not found" });

            return Ok(profile);
        }

        // ✅ POST: api/User/profile
        [HttpPost("profile")]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveProfile([FromForm] SaveUserProfileRequest model)
        {
            await _userService.SaveProfileAsync(model, model.ProfileImage);
            return Ok(new { Message = "Profile saved successfully" });
        }
    }
}
