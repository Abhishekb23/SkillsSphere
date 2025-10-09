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

            // Explicit column selection + alias prevents ambiguous column errors
            var sql = @"
        SELECT 
            u.""UserId"",
            u.""Username"",
            u.""Email"",
            u.""PasswordHash"",
            u.""Role"",
            u.""IsActive"",
            u.""CreatedAt"",
            u.""UpdatedAt""
        FROM get_user_by_id(@p_userid) AS u;
    ";

            // The parameter name @p_userid matches the function argument
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { p_userid = id });
        }


        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var conn = GetConnection();
            var sql = "SELECT * FROM get_all_users()";
            return await conn.QueryAsync<User>(sql);
        }


        public async Task AddUserAsync(User user)
        {
            using var conn = GetConnection();
            var p = new DynamicParameters();
            p.Add("p_username", user.Username);
            p.Add("p_email", user.Email);
            p.Add("p_passwordhash", user.PasswordHash);
            p.Add("p_role", (int)user.Role);
            p.Add("p_userid", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("create_user", p, commandType: CommandType.StoredProcedure);

            user.UserId = p.Get<int>("p_userid");
        }

        public async Task UpdateUserAsync(User user)
        {
            using var conn = GetConnection();
            var sql = @"
                UPDATE ""Users""
                SET ""Username"" = @Username,
                    ""Email"" = @Email,
                    ""PasswordHash"" = @PasswordHash,
                    ""Role"" = @Role,
                    ""IsActive"" = @IsActive,
                    ""UpdatedAt"" = @UpdatedAt
                WHERE ""UserId"" = @UserId";

            await conn.ExecuteAsync(sql, new
            {
                user.Username,
                user.Email,
                user.PasswordHash,
                Role = (int)user.Role,
                user.IsActive,
                user.UpdatedAt,
                user.UserId
            });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var conn = GetConnection();
            var sql = @"SELECT * FROM ""Users"" WHERE ""Email"" = @Email";
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }
    }
}
