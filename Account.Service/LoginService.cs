using Account.Service.Intefaces;
using Account.Service.Models;
using Exceptions;
using System;
using System.Threading.Tasks;

namespace Account.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<Guid> LoginAsync(string email, string password)
        {
            bool isCustomerExist = await _loginRepository.IsCustomerExistAsync(email, password);
            if (isCustomerExist)
            {
                return await _loginRepository.LoginAsync(email);
            }
            else
            {
                throw new AccountNotFoundException();
            }
        }

        public async Task<bool> RegisterAsync(CustomerModel customerModel)
        {
            bool isVerificationCodeValid = await _loginRepository.IsVerificationCodeValidAsync(customerModel.VerificationCode, customerModel.Email);
            bool isEmailValid = await _loginRepository.IsEmailValidAsync(customerModel.Email);
            if (isEmailValid && isVerificationCodeValid)
            {
                string passowrdSalt = Hashing.GetSalt();
                //customerModel.Id = Guid.NewGuid();
                customerModel.PassowrdSalt = passowrdSalt;
                customerModel.PasswordHash = Hashing.GenerateHash(customerModel.Password, passowrdSalt);
                AccountRegisterModel account = new AccountRegisterModel()
                {
                    //CustomerId = customerModel.Id,
                    Balance = 10000
                };
                return await _loginRepository.RegisterAsync(customerModel, account);
            }
            if (isEmailValid == false)
            {
                throw new DuplicateEmailException();
            }
            if (isVerificationCodeValid == false)
            {
                throw new VerificationCodeNotValidException();
            }
            return false;
        }
    }

}

