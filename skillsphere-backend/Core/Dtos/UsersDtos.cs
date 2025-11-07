
using static skillsphere.core.Entities.Constants;

namespace skillsphere.core.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.Learner;
        public bool IsActive { get; set; } = true;
    }

    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class VerifyOtpDto
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }


}
