using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string otp);
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendRegistrationSuccessAsync(string toEmail, string username);
    }
}
