using Account.Service.Intefaces;
using Account.Service.Models;
using Account.WebApi.DTO;
using AutoMapper;
using Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Account.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;
        public LoginController(ILoginService loginService, IMapper mapper, IMessageSession messageSession)
        {
            _loginService = loginService;
            _mapper = mapper;
            _messageSession = messageSession;
        }

        [HttpPost("login")]
        public async Task<Guid> LoginAsync([FromBody] LoginDTO login)
        {
            Guid accountId = await _loginService.LoginAsync(login.Email, login.Password);
            return accountId;
        }

        [HttpPost("register")]
        public async Task<bool> RegisterAsync([FromBody] CustomerDTO customer)
        {
            CustomerModel customerModel = _mapper.Map<CustomerModel>(customer);
            var response = await _loginService.RegisterAsync(customerModel);
            return response;
        }
        [HttpPost("sendEmail")]
        public async Task <bool> SendEmailAsync ([FromBody] EmailDTO email)
        {
            SendEmail sendEmail = new SendEmail()
            {
                Email = email.Email
            };
            await _messageSession.Send(sendEmail)
                .ConfigureAwait(false);
            return true;
        }
    }
}
