using Account.Service.Intefaces;
using Account.Service.Models;
using Account.WebApi.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public LoginController(ILoginService loginService, IMapper mapper)
        {
            _loginService = loginService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Guid>> LoginAsync([FromBody] LoginDTO login)
        {
            Guid accountId = await _loginService.LoginAsync(login.Email, login.Password);
            return Ok(accountId);
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> RegisterAsync([FromBody] CustomerDTO customer)
        {
            CustomerModel customerModel = _mapper.Map<CustomerModel>(customer);
            var response = await _loginService.RegisterAsync(customerModel);
            return Ok(response);
        }
    }
}
