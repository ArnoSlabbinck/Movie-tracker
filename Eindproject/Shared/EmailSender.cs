using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Eindproject.Domain;
using Microsoft.AspNetCore.Identity;
using System;

namespace Movietracker.Shared
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
     
        public EmailSender(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _config = configuration;
            
        }


        public async Task SendEmailAsync(string email,  string subject, 
            string htmlMessage)
        {
            var NameFrom = _config.GetValue<string>("Email_From");
            var emailAddressFrom = _config.GetValue<string>("Email_SendGrid");
            EmailAddress EmailFrom = new EmailAddress() { Email = emailAddressFrom, Name = NameFrom };
            var apiKey = _config.GetValue<string>("SendGrid_ApiKey");
            var client = new SendGridClient(apiKey);
            var plainTextContent = "You can renew your password";
            var msg = new SendGridMessage()
            {
                From = EmailFrom,
                Subject = subject,
                PlainTextContent = plainTextContent,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));
            

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode.ToString());
            Console.WriteLine(response.Headers.Via);
            Console.WriteLine(response.Headers.Connection);
            Console.WriteLine(response.Body.ToString());





        }

       
    }
}
