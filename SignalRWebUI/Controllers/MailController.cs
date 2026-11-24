using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SignalR.DtoLayer.Dtos.MailDtos;

namespace SignalRWebUI.Controllers
{
    public class MailController : Controller
    {
        private readonly IConfiguration _configuration;

        public MailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(CreateMailDto dto)
        {
            try
            {
                var admin = _configuration.GetSection("Admin");
                var smtp = _configuration.GetSection("Smtp");
                var name = admin["Name"];
                var mail = admin["Mail"];
                var key = admin["Key"];

                var smtpMail = smtp["Mail"];
                var port = smtp["Port"];

                var mimeMessage = new MimeMessage();
                var mailBoxAddressFrom = new MailboxAddress(name, mail);
                mimeMessage.From.Add(mailBoxAddressFrom);

                var mailBoxAddressTo = new MailboxAddress(dto.ReceiveName, dto.ReceiveMail);
                mimeMessage.To.Add(mailBoxAddressTo);

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = dto.Body;
                mimeMessage.Body = bodyBuilder.ToMessageBody();
                mimeMessage.Subject = dto.Subject;

                var client = new SmtpClient();
                client.Connect(smtpMail, Convert.ToInt32(port), false);
                client.Authenticate(mail, key);
                client.Send(mimeMessage);
                client.Disconnect(true);

                TempData["MailSuccess"] = "Mail gönderildi.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Mail gönderilemedi. Lütfen bilgileri kontrol edip tekrar deneyin.";
                return View(dto);
            }
        }
    }
}
