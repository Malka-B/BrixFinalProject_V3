using Account.Data.Entites;
using Account.Service;
using Account.Service.Intefaces;
using Account.Service.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Account.Data
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;

        public LoginRepository(AccountContext accountContext, IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }

        public async Task<bool> IsEmailValidAsync(string email)
        {
            CustomerEntity customerEntity = await _accountContext.Customers
                .FirstOrDefaultAsync(s => s.Email == email);
            if (customerEntity != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsCustomerExistAsync(string email, string password)
        {
            CustomerEntity customer = await _accountContext.Customers
             .FirstOrDefaultAsync(c => c.Email == email);
            if (customer != null)
            {
                if (Hashing.AreEqual(password, customer.PasswordHash, customer.PassowrdSalt))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<Guid> LoginAsync(string email)
        {
            CustomerEntity customer = await _accountContext.Customers
                .FirstOrDefaultAsync(c => c.Email == email);

            AccountEntity account = await _accountContext.Accounts
                .FirstOrDefaultAsync(a => a.CustomerId == customer.Id);
            return account.Id;
        }

        public async Task<bool> RegisterAsync(CustomerModel customerModel, AccountRegisterModel accountRegisterModel)
        {
            CustomerEntity customer = _mapper.Map<CustomerEntity>(customerModel);
            AccountEntity account = _mapper.Map<AccountEntity>(accountRegisterModel);
            account.Id = new Guid();
            account.OpenDate = DateTime.Now;
            await _accountContext.Customers.AddAsync(customer);
            await _accountContext.Accounts.AddAsync(account);
            await _accountContext.SaveChangesAsync();
            return true;
        }
        private string GenerateOTP()
        {
            int _min = 1000, _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max).ToString();
        }

        public async Task<string> AddEmailVerifcationAsync(string email)
        {
            EmailVerificationEntity emailVerificationEntity =
              await _accountContext.EmailVerification.FirstOrDefaultAsync(e => e.Email == email);
            if (emailVerificationEntity == null)
            {
                emailVerificationEntity = new EmailVerificationEntity()
                {
                    Id = new Guid(),
                    Email = email,
                    ExpirationTime = DateTime.Now.AddMinutes(5),
                    VerificationCode = GenerateOTP()
                };
                await _accountContext.EmailVerification.AddAsync(emailVerificationEntity);
            }
            else
            {
                emailVerificationEntity.ExpirationTime = DateTime.Now.AddMinutes(5);
                emailVerificationEntity.VerificationCode = GenerateOTP();
            }
            await _accountContext.SaveChangesAsync();
            return emailVerificationEntity.VerificationCode;
        }

        public async Task<bool> IsVerificationCodeValidAsync(string verificationCode,string email)
        {
            EmailVerificationEntity emailVerification = await _accountContext
                   .EmailVerification.FirstOrDefaultAsync(e => e.Email == email
                   && e.ExpirationTime > DateTime.Now && e.VerificationCode == verificationCode);
            if (emailVerification != null)
                return true;
            return false;
        }
    }
}

