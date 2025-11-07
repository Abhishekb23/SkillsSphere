using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace skillsphere.infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly SendGridClient _client;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
            var apiKey = string.IsNullOrWhiteSpace(_emailSettings.ApiKey)
                ? Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                : _emailSettings.ApiKey;

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("SendGrid API key not configured.");

            _client = new SendGridClient(apiKey);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var from = new EmailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, body);

            var response = await _client.SendEmailAsync(msg);
            Console.WriteLine($"📤 SendGrid response: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"❌ SendGrid error: {error}");
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }
        }

        public async Task SendOtpAsync(string toEmail, string otp)
        {
            string subject = "SkillSphere Email Verification (OTP)";
            string body = $@"
                <html>
                <body style='font-family:Segoe UI,Roboto,Arial,sans-serif; background:#f4f7fb; padding:20px;'>
                  <div style='max-width:600px;margin:auto;background:#ffffff;border-radius:8px;padding:25px;box-shadow:0 4px 10px rgba(0,0,0,0.08);'>
                    <h2 style='color:#2563eb;text-align:center;'>SkillSphere Email Verification</h2>
                    <p style='font-size:16px;color:#333;'>Your verification code is:</p>
                    <div style='text-align:center;margin:25px 0;'>
                      <span style='font-size:32px;letter-spacing:6px;color:#111;font-weight:bold;'>{otp}</span>
                    </div>
                    <p style='font-size:14px;color:#555;'>This code is valid for 5 minutes.</p>
                    <p style='font-size:13px;color:#777;text-align:center;'>Thanks for joining SkillSphere 💙</p>
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
                    <p style='font-size:15px;color:#333;'>Your registration was successful! You can now log in and explore our platform.</p>
                    <p style='color:#777;font-size:13px;text-align:center;'>© {DateTime.UtcNow.Year} SkillSphere</p>
                  </div>
                </body>
                </html>";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
