using Account.Service.Intefaces;
using Messages.Commands;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Account.NSB
{
    public class SendEmailHandler : IHandleMessages<SendEmail>
    {
        private readonly ILoginRepository _loginRepository;        
        private IConfigurationRoot ConfigRoot;

        public SendEmailHandler(ILoginRepository loginRepository, IConfigurationRoot configuration)
        {
            _loginRepository = loginRepository;
            ConfigRoot = configuration;           
        }
        public async Task Handle(SendEmail NSBMessage, IMessageHandlerContext context)
        {
            string d = ConfigRoot.GetConnectionString("RabbitMQ");            
            string verificationCode = await _loginRepository.AddEmailVerifcationAsync(NSBMessage.Email);
            var fromAddress = new MailAddress(GetMailInfo("sourceMail"), GetMailInfo("sourceMailName"));
            var toAddress = new MailAddress(NSBMessage.Email);
            string fromPassword = GetMailInfo("sourceMailPassword");
            string subject = GetMailInfo("mailSubject");
            string body = $"{GetMailInfo("mailBodyBeforeCode")} { verificationCode.ToString()} \n {GetMailInfo("mailBodyAfterCode")}";
            var smtp = new SmtpClient
            {
                Host = GetSmtpInfo("smtpHost"),
                Port = Int32.Parse(GetSmtpInfo("smtpPort")),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            string GetMailInfo(string mailInfo)
            {
                return ConfigRoot.GetSection("Email").GetSection(mailInfo).Value;
            }

            string GetSmtpInfo(string smtpInfo)
            {
                return ConfigRoot.GetSection("Smtp").GetSection(smtpInfo).Value;
            }
        }
    }
}
