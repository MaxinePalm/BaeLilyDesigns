using System.Net;
using System.Net.Mail;

namespace BaeLilyDesigns.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmail(string to, string subject, string htmlBody)
        {
            var smtpHost = _config["Email:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "587");
            var fromEmail = _config["Email:From"] ?? "";
            var password = _config["Email:Password"] ?? "";

            // If email isn't configured, just log it (dev mode)
            if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine($"[EMAIL - not configured] To: {to} | Subject: {subject}");
                return;
            }

            var smtp = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail, "Bae Lily Designs"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mail.To.Add(to);

            await smtp.SendMailAsync(mail);
        }

        public async Task SendOrderConfirmation(string to, string customerName, int orderId, decimal total, List<(string name, string size, int qty, decimal price)> items)
        {
            var itemRows = string.Join("", items.Select(i =>
                $"<tr><td style='padding:8px;border-bottom:1px solid #eee;'>{i.name} ({i.size})</td>" +
                $"<td style='padding:8px;border-bottom:1px solid #eee;text-align:center;'>{i.qty}</td>" +
                $"<td style='padding:8px;border-bottom:1px solid #eee;text-align:right;'>R{i.price * i.qty:N0}</td></tr>"));

            var body = $@"
<!DOCTYPE html>
<html>
<body style='font-family:Georgia,serif;background:#faf9f7;margin:0;padding:0;'>
  <div style='max-width:600px;margin:40px auto;background:#fff;border-radius:12px;overflow:hidden;box-shadow:0 2px 20px rgba(0,0,0,0.08);'>
    <div style='background:#2A2118;padding:40px;text-align:center;'>
      <h1 style='color:#D4B896;font-family:Georgia,serif;letter-spacing:3px;margin:0;font-size:1.8rem;'>BAE LILY DESIGNS</h1>
      <p style='color:#F5F0E8;margin:8px 0 0;font-size:0.9rem;letter-spacing:1px;'>Order Confirmation</p>
    </div>
    <div style='padding:40px;'>
      <h2 style='color:#2A2118;'>Thank you, {customerName}! 🌸</h2>
      <p style='color:#555;line-height:1.6;'>Your pre-order has been received. We'll begin crafting your items within 48 hours and ship within 2–3 weeks.</p>
      <p style='color:#555;'><strong>Order #</strong> {orderId}</p>
      <table style='width:100%;border-collapse:collapse;margin:24px 0;'>
        <thead>
          <tr style='background:#f5f0e8;'>
            <th style='padding:10px;text-align:left;color:#2A2118;'>Item</th>
            <th style='padding:10px;text-align:center;color:#2A2118;'>Qty</th>
            <th style='padding:10px;text-align:right;color:#2A2118;'>Price</th>
          </tr>
        </thead>
        <tbody>{itemRows}</tbody>
        <tfoot>
          <tr>
            <td colspan='2' style='padding:12px;font-weight:bold;color:#2A2118;'>Total</td>
            <td style='padding:12px;text-align:right;font-weight:bold;color:#2A2118;font-size:1.1rem;'>R{total:N0}</td>
          </tr>
        </tfoot>
      </table>
      <p style='color:#555;line-height:1.6;'>You'll receive a shipping notification with tracking once your order is on its way.</p>
      <p style='color:#555;'>Made in South Africa with love 🇿🇦</p>
    </div>
    <div style='background:#f5f0e8;padding:20px;text-align:center;'>
      <p style='color:#8B6F47;font-size:0.85rem;margin:0;'>© 2026 Bae Lily Designs. All rights reserved.</p>
    </div>
  </div>
</body>
</html>";

            await SendEmail(to, $"Order #{orderId} Confirmed – Bae Lily Designs", body);
        }
    }
}
