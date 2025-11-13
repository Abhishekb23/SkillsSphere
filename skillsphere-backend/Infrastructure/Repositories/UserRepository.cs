using Dapper;
using System.Data;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.infrastructure.Data;

namespace skillsphere.infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _dbContext;

        public UserRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IDbConnection GetConnection() => _dbContext.CreateConnection();

        public async Task<User?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();

            string sql = @"SELECT * FROM get_user_by_id(@p_userid)";
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { p_userid = id });
        }



        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<User>("SELECT * FROM get_all_users()");
        }


        public async Task AddUserAsync(User user)
        {
            using var conn = GetConnection();

            var sql = "SELECT create_user(@p_username, @p_email, @p_passwordhash, @p_role)";

            var userId = await conn.ExecuteScalarAsync<int>(sql, new
            {
                p_username = user.Username,
                p_email = user.Email,
                p_passwordhash = user.PasswordHash,
                p_role = (int)user.Role
            });

            user.UserId = userId;
        }



        public async Task UpdateUserAsync(User user)
        {
            using var conn = GetConnection();

            await conn.ExecuteAsync(
                "CALL update_user(@p_userid, @p_username, @p_email, @p_passwordhash, @p_role, @p_isactive)",
                new
                {
                    p_userid = user.UserId,
                    p_username = user.Username,
                    p_email = user.Email,
                    p_passwordhash = user.PasswordHash,
                    p_role = (int)user.Role,
                    p_isactive = user.IsActive
                });
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM get_user_by_email(@Email);",
                new { Email = email });
        }


        public async Task<User?> GetByEmailOrUsernameAsync(string identifier)
        {
            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM get_user_by_email_or_username(@Identifier);",
                new { Identifier = identifier });
        }
    }
}
