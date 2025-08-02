using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using parcelPicUp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelPicUp.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send([Bind("Name, Email, Subject, Message")] PContact pContact)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", pContact);
            }

            var contactConfig = await _context.ContactConfig.FirstOrDefaultAsync();

            if (contactConfig != null)
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(contactConfig.FormName, contactConfig.FormEmail));
                message.To.Add(new MailboxAddress(contactConfig.ToName, contactConfig.ToEmail));
                message.Subject = pContact.Subject;

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = $"Name: {pContact.Name}\r\nEmail: {pContact.Email}\r\n\r\n{pContact.Message}"
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                try
                {
                    SecureSocketOptions socketOption = contactConfig.SMTPPort switch
                    {
                        465 => SecureSocketOptions.SslOnConnect,
                        587 => SecureSocketOptions.StartTls,
                        _ => SecureSocketOptions.Auto,
                    };

                    await client.ConnectAsync(contactConfig.SMTPHost, contactConfig.SMTPPort, socketOption);

                    await client.AuthenticateAsync(contactConfig.FormEmail, contactConfig.FormEmailPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                    TempData["Success"] = "Your message has been sent successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Email send failed: {ex.Message}");
                    return View("Index", pContact);
                }
            }

            ModelState.AddModelError(string.Empty, "Email configuration not found.");
            return View("Index", pContact);
        }

        public class PContact
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }
    }
}
