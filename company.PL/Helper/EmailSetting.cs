using company.PL.Settings;
using Company.DAL.models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace company.PL.Helper
{
    public class EmailSetting:IMailSetting
    {
        private MailSetting _options;

        //public static void SendEmail(Email email)
        //{
        //    var client = new SmtpClient("smtp.gmail.com",587);
        //    client.EnableSsl = true;
        //    client.Credentials = new NetworkCredential("rahmaroute1@gmail.com", "hkkx hkbs mnns xbvo");
        //    client.Send("rahmaroute1@gmail.com",email.To,email.Subject,email.Body);


        //}
        public EmailSetting(IOptions<MailSetting> options)
        {
            _options = options.Value;
        }
        public void SendMail(Email email)
        {
            //sender
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };
            //Receiver send to who?
            mail.To.Add(MailboxAddress.Parse(email.To));

            //body
            var builder= new BodyBuilder();
            builder.TextBody = email.Body;

            mail.Body=builder.ToMessageBody();
            mail.From.Add(new MailboxAddress(_options.DisplayName,_options.Email));

            //open connection
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);

            smtp.Send(mail);

            smtp.Disconnect(true);






        }
    }
}
