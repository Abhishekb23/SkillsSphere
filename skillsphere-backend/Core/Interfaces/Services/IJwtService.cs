using skillsphere.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
