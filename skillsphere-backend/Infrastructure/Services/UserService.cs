using AutoMapper;
using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.core.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;


namespace skillsphere.infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IMapper mapper, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
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
                throw new Exception("User already exists with this email");

            user.PasswordHash = dto.Password;

            await _userRepository.AddUserAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid email or password");

            // For now, plain text password (replace with hashing in production)
            if (user.PasswordHash != password)
                throw new Exception("Invalid email or password");

            // Generate JWT token
            return _jwtService.GenerateToken(user);
        }
    }
}
