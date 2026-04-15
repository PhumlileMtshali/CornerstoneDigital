using System.Net;
using System.Net.Mail;

namespace CornerstoneDigital.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationEmail(string toEmail, string customerName, string orderReference, string packageName, decimal amount);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendOrderConfirmationEmail(string toEmail, string customerName, string orderReference, string packageName, decimal amount)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var password = _configuration["EmailSettings:Password"];

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(senderEmail, password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail, senderName),
                        Subject = $"Order Confirmation - {orderReference}",
                        Body = GetEmailBody(customerName, orderReference, packageName, amount),
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Confirmation email sent to {toEmail}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}");
                throw;
            }
        }

        private string GetEmailBody(string customerName, string orderReference, string packageName, decimal amount)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #6B2E3E 0%, #4A1F2C 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .order-box {{ background: white; padding: 20px; margin: 20px 0; border-left: 4px solid #C9A961; border-radius: 5px; }}
                        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
                        .button {{ display: inline-block; padding: 12px 30px; background: #6B2E3E; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                        h1 {{ margin: 0; font-size: 28px; }}
                        h3 {{ color: #6B2E3E; margin-top: 0; }}
                        .checkmark {{ color: #28a745; font-size: 50px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <div class='checkmark'>✓</div>
                            <h1>Payment Successful!</h1>
                            <p style='margin: 10px 0 0 0;'>Thank you for choosing Cornerstone Digital</p>
                        </div>
                        <div class='content'>
                            <p>Dear {customerName},</p>
                            <p>We're excited to confirm that your payment has been received and your order has been successfully placed!</p>
                            
                            <div class='order-box'>
                                <h3>📋 Order Details</h3>
                                <p style='margin: 5px 0;'><strong>Order Reference:</strong> {orderReference}</p>
                                <p style='margin: 5px 0;'><strong>Package:</strong> {packageName}</p>
                                <p style='margin: 5px 0;'><strong>Amount Paid:</strong> ${amount:N2}</p>
                                <p style='margin: 5px 0;'><strong>Date:</strong> {DateTime.Now:MMMM dd, yyyy}</p>
                            </div>

                            <h3>🎯 What Happens Next?</h3>
                            <ol style='line-height: 1.8;'>
                                <li><strong>Within 2 Hours:</strong> You'll receive a welcome email with next steps</li>
                                <li><strong>Within 24 Hours:</strong> Your dedicated project manager will contact you</li>
                                <li><strong>Project Kickoff:</strong> We'll schedule a consultation call to discuss details</li>
                                <li><strong>Project Begins:</strong> Our team starts working on your project right away</li>
                            </ol>

                            <div style='background: #fff3cd; padding: 15px; border-radius: 5px; border-left: 4px solid #ffc107; margin: 20px 0;'>
                                <strong>📞 Need Help?</strong><br>
                                Our support team is here for you:<br>
                                Email: support@cornerstonedigital.com<br>
                                Phone: +27 XX XXX XXXX
                            </div>

                            <p style='text-align: center;'>
                                <a href='https://localhost:7050' class='button'>Visit Our Website</a>
                            </p>

                            <p style='margin-top: 30px;'>Thank you for your business!</p>
                            <p><strong>The Cornerstone Digital Team</strong></p>
                        </div>
                        <div class='footer'>
                            <p><strong>Cornerstone Digital</strong></p>
                            <p>support@cornerstonedigital.com | +27 XX XXX XXXX</p>
                            <p>&copy; 2026 Cornerstone Digital. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }
    }
}