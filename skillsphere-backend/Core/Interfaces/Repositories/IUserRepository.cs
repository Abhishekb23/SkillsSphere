using skillsphere.core.Entities;

namespace skillsphere.core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByEmailOrUsernameAsync(string identifier)
        Task<IEnumerable<User>> GetAllAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
