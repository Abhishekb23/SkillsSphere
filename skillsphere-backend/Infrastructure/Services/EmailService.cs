using skillsphere.core.Interfaces.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace skillsphere.infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail = "skills.sphere.v1@gmail.com"; // your Gmail address
        private readonly string _appPassword = "gojtytnrudkbkclk";        // your Gmail App Password

        /// <summary>
        /// Sends an email using Gmail SMTP.
        /// </summary>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(_fromEmail, _appPassword);
                smtp.EnableSsl = true;

                var mail = new MailMessage
                {
                    From = new MailAddress(_fromEmail, "SkillSphere"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false // true if sending HTML content
                };

                mail.To.Add(toEmail);

                try
                {
                    await smtp.SendMailAsync(mail);
                    Console.WriteLine($"✅ Email sent successfully to {toEmail}");
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"❌ Email sending failed: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sends OTP email for user verification.
        /// </summary>
        public async Task SendOtpAsync(string toEmail, string otp)
        {
            string subject = "SkillSphere Email Verification (OTP)";
            string body = $"Hello,\n\nYour SkillSphere verification code is: {otp}\n\n" +
                          "This code is valid for 5 minutes.\n\nThank you,\nTeam SkillSphere";

            await SendEmailAsync(toEmail, subject, body);
        }

        /// <summary>
        /// Sends registration success confirmation email.
        /// </summary>
        public async Task SendRegistrationSuccessAsync(string toEmail, string username)
        {
            string subject = "Welcome to SkillSphere 🎉";
            string body =
                $"Hello {username},\n\n" +
                "🎉 Congratulations! Your SkillSphere registration was successful.\n\n" +
                "You can now log in and start exploring our platform.\n\n" +
                "Best regards,\nTeam SkillSphere";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
