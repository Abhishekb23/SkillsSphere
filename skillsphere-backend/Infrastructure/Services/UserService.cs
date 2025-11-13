using AutoMapper;
using Microsoft.AspNetCore.Http;
using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.core.Interfaces.Services;
using skillsphere.infrastructure.Repositories;


namespace skillsphere.infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationRepository _verificationRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            IVerificationRepository verificationRepository,
            IEmailService emailService,
            IMapper mapper,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _verificationRepository = verificationRepository;
            _emailService = emailService;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        // ✅ Send OTP for registration
        public async Task SendRegistrationOtpAsync(CreateUserDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("User already exists");

            // Delete any old pending record
            await _verificationRepository.DeletePendingAsync(dto.Email);

            var otp = new Random().Next(100000, 999999).ToString();

            await _verificationRepository.AddPendingUserAsync(
                dto.Username, dto.Email, dto.Password, 1, otp);

            await _emailService.SendOtpAsync(dto.Email, otp);
        }

        // ✅ Verify OTP and register user
        public async Task<UserDto> VerifyOtpAndRegisterAsync(string email, string otp)
        {
            var pending = await _verificationRepository.GetPendingByEmailAsync(email);

            if (pending == null)
                throw new Exception("No registration found for this email or OTP expired.");

            if (pending.OtpCode.Trim() != otp.Trim())
                throw new Exception("Invalid OTP.");

            if (pending.ExpiresAt < DateTime.UtcNow)
                throw new Exception("OTP expired. Please register again.");

            // ✅ Registration success — move to Users table
            var user = new User
            {
                Username = pending.Username,
                Email = pending.Email,
                PasswordHash = pending.PasswordHash,
                Role = (skillsphere.core.Entities.Constants.UserRole)pending.Role,
                IsActive = true
            };

            await _userRepository.AddUserAsync(user);
            await _verificationRepository.DeletePendingAsync(email);
            await _emailService.SendRegistrationSuccessAsync(user.Email, user.Username);


            return _mapper.Map<UserDto>(user);

        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var entity = await _userRepository.GetByEmailAsync(user.Email);
            if (entity != null)
                throw new Exception("User already exists...");

            user.PasswordHash = dto.Password;
            user.Role=Constants.UserRole.Admin;

            await _userRepository.AddUserAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailOrUsernameAsync(email);
            if (user == null)
                throw new Exception("Invalid Credentials....");

            // For now, plain text password (replace with hashing in production)
            if (user.PasswordHash != password)
                throw new Exception("Invalid Password");

            // Generate JWT token
            return _jwtService.GenerateToken(user);
        }

        public Task<UserProfileDto?> GetProfileAsync(int userId)
            => _userRepository.GetProfileAsync(userId);

        public async Task SaveProfileAsync(SaveUserProfileRequest model, IFormFile? profileImage)
        {
            byte[]? bytes = null;

            if (profileImage != null)
            {
                using var ms = new MemoryStream();
                await profileImage.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            await _userRepository.SaveProfileAsync(model, bytes);
        }
    }
}
