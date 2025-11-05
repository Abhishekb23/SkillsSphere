using skillsphere.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Interfaces.Repositories
{
    public interface IVerificationRepository
    {
        Task<PendingUserVerification?> GetPendingByEmailAsync(string email);
        Task AddPendingUserAsync(string username, string email, string passwordHash, int role, string otp);
        Task DeletePendingAsync(string email);
    }
}
