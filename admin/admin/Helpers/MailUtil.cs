using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace admin.Helpers
{
    public class MailUtil
    {
        private static MailUtil _instance;

        private MailUtil() { }

        public static MailUtil GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MailUtil();
            }

            return _instance;
        }

        public async Task<bool> SendGmail(string to, string subject, string body)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Gmail");

            MailMessage mailMessage = new MailMessage();
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            mailMessage.From = new MailAddress(config["From"]);
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            using var smtpClient = new SmtpClient(config["Host"], int.Parse(config["Port"]))
            {
                Credentials = new NetworkCredential(config["UserName"], config["Password"]),
                EnableSsl = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}