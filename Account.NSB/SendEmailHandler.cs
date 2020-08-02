using Account.Service.Intefaces;
using Messages.Commands;
using NServiceBus;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Account.NSB
{
    public class SendEmailHandler : IHandleMessages<SendEmail>
    {
        private readonly ILoginRepository _loginRepository;
        public SendEmailHandler(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public async Task Handle(SendEmail NSBMessage, IMessageHandlerContext context)
        {
            string verificationcode = await _loginRepository.AddEmailVerifcationAsync(NSBMessage.Email);
            var fromAddress = new MailAddress("crossrivermb@gmail.com", "CrossRiver no-reply");
            var toAddress = new MailAddress(NSBMessage.Email);
            const string fromPassword = "aa11bb22";
            const string subject = "Verification Code";
            string body = $"Hi, here your Verification Code:\n { verificationcode.ToString()}";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
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
        }
    }
}

