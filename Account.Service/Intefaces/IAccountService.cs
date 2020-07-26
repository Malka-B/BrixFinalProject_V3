using Account.Service.Models;
using Account.Share.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Service.Intefaces
{
    public interface IAccountService
    {
        Task<AccountModel> GetAccountInfoAsync(Guid CustomerId);
    }
}
