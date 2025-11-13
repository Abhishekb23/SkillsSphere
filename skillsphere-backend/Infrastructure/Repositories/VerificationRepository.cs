using Dapper;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.infrastructure.Data;
using System.Data;

namespace skillsphere.infrastructure.Repositories
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly DatabaseContext _dbContext;

        public VerificationRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IDbConnection GetConnection() => _dbContext.CreateConnection();

        public async Task AddPendingUserAsync(string username, string email, string passwordHash, int role, string otp)
        {
            using var conn = GetConnection();

            await conn.ExecuteAsync(
                "CALL add_pending_user(@Username, @Email, @PasswordHash, @Role, @OtpCode, @ExpiresAt)",
                new
                {
                    Username = username,
                    Email = email,
                    PasswordHash = passwordHash,
                    Role = role,
                    OtpCode = otp,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(2)
                });
        }


        public async Task<PendingUserVerification?> GetPendingByEmailAsync(string email)
        {
            using var conn = GetConnection();

            const string sql = @"SELECT * FROM get_pending_user_by_email(@Email);";

            return await conn.QuerySingleOrDefaultAsync<PendingUserVerification>(sql, new { Email = email });
        }



        public async Task DeletePendingAsync(string email)
        {
            using var conn = GetConnection();

            await conn.ExecuteAsync(
                "CALL delete_pending_user(@Email)",
                new { Email = email });
        }

    }
}
