using Account.Service.Models;
using System;
using System.Threading.Tasks;

namespace Account.Service.Intefaces
{
    public interface ILoginService
    {
        
        Task<Guid> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(CustomerModel customerModel);
       // Task<bool> SendEmailasync(string email);
    }
}
