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

            const string sql = @"
                INSERT INTO ""PendingUserVerifications""
                (""Username"", ""Email"", ""PasswordHash"", ""Role"", ""OtpCode"", ""ExpiresAt"")
                VALUES (@Username, @Email, @PasswordHash, @Role, @OtpCode, @ExpiresAt)";

            await conn.ExecuteAsync(sql, new
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                OtpCode = otp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            });
        }

        public async Task<PendingUserVerification?> GetPendingByEmailAsync(string email)
        {
            using var conn = GetConnection();

            string sql = @"SELECT * FROM ""PendingUserVerifications"" WHERE ""Email"" = @Email";
            return await conn.QuerySingleOrDefaultAsync<PendingUserVerification>(sql, new { Email = email });
        }


        public async Task DeletePendingAsync(string email)
        {
            using var conn = GetConnection();
            await conn.ExecuteAsync(@"DELETE FROM ""PendingUserVerifications"" WHERE ""Email"" = @Email", new { Email = email });
        }
    }
}
