using Microsoft.Extensions.Options;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace skillsphere.infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            using var smtp = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true
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

        public async Task SendOtpAsync(string toEmail, string otp)
        {
            string subject = "Verify Your SkillSphere Account (OTP)";
            string body = $@"
                <html>
                <body style='font-family:Segoe UI,Roboto,Arial,sans-serif; background:#f4f7fb; padding:20px;'>
                  <div style='max-width:600px;margin:auto;background:#ffffff;border-radius:8px;padding:25px;box-shadow:0 4px 10px rgba(0,0,0,0.08);'>
                    <h2 style='color:#2563eb;text-align:center;'>SkillSphere Email Verification</h2>
                    <p style='font-size:16px;color:#333;'>Your OTP code is:</p>
                    <div style='text-align:center;margin:25px 0;'>
                      <span style='font-size:32px;letter-spacing:6px;color:#111;font-weight:bold;'>{otp}</span>
                    </div>
                    <p style='font-size:14px;color:#555;'>This code is valid for 5 minutes.</p>
                  </div>
                </body>
                </html>";
            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendRegistrationSuccessAsync(string toEmail, string username)
        {
            string subject = "Welcome to SkillSphere 🎉";
            string body = $@"
                <html>
                <body style='font-family:Segoe UI,Roboto,Arial,sans-serif; background:#f4f7fb; padding:20px;'>
                  <div style='max-width:600px;margin:auto;background:#ffffff;border-radius:8px;padding:25px;box-shadow:0 4px 10px rgba(0,0,0,0.08);'>
                    <h2 style='color:#2563eb;text-align:center;'>Welcome to SkillSphere 🎉</h2>
                    <p style='font-size:16px;color:#333;'>Hi {username},</p>
                    <p style='font-size:15px;color:#333;'>Your registration was successful!</p>
                    <p style='color:#777;font-size:13px;text-align:center;'>© {DateTime.UtcNow.Year} SkillSphere</p>
                  </div>
                </body>
                </html>";
            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
