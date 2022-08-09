using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebAPI.Contexts;
using WebAPI.Misc;
using WebAPI.Models.Database;
using WebAPI.Services.Models;

namespace WebAPI.Services
{
    public class MailService
    {

        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly VerificationService _verification;
        private readonly TemplateService _templateService;

        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly string _fromMail;
        private readonly string _fromMailName;

        public MailService(DatabaseContext context, IConfiguration configuration, VerificationService verification, TemplateService templateService)
        {
            _context = context;
            _configuration = configuration;
            _verification = verification;
            _templateService = templateService;

            var section = _configuration.GetSection("Email");
            _host = section["Host"];
            _username = section["User"];
            _password = section["Password"];
            _fromMail = section["FromAddress"];
            _fromMailName = section["Name"];
        }

        public async Task<bool> SendConfirmationMail(User user)
        {
            if (!_verification.VerifyMail(user.Email)) return false;
            
            var code = KeyGenerator.Generate();

            user.VerificationCode = code;
            _context.Update(user);
            await _context.SaveChangesAsync();
            
            SendConfirmationMail(new MailAddress(user.Email, user.Name), code);

            return true;
        }

        private void SendConfirmationMail(MailAddress to, string code)
        {
            TemplateParameter codePara = new TemplateParameter("code", code);
            SendMail(to,
                _templateService.GetConfirmationMailSubject(),
                _templateService.GetConfirmationMail(codePara));
        }
        
        private void SendMail(MailAddress to, string subject, string html)
        {
            var section = _configuration.GetSection("Email");
            if (section["Enable"] != "true") return;
            if (section["Redirect"] == "true")
                to = new MailAddress(section["RedirectAddress"], to.DisplayName);
            
            try
            {
                MailMessage mailMsg = new MailMessage();
                
                mailMsg.To.Add(to);
                mailMsg.From = new MailAddress(_fromMail, _fromMailName);
                
                mailMsg.Subject = subject;
                mailMsg.AlternateViews.Add(AlternateView
                    .CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                
                SmtpClient smtpClient = new SmtpClient(_host, Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = 
                    new System.Net.NetworkCredential(_username, _password);
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}