using Account.Service.Models;
using Account.WebApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Account.Data.Entites;
using Account.Share.Models;

namespace Account.WebApi.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountModel, AccountDTO>();
            CreateMap<AccountDTO, AccountModel>();
            CreateMap<AccountEntity, AccountModel>()
             .ForMember(destination => destination.FirstName, option => option.MapFrom(src =>
             src.Customer.FirstName))
             .ForMember(destination => destination.LastName, option => option.MapFrom(src =>
              src.Customer.LastName));
            CreateMap<CustomerModel, CustomerDTO>();
            CreateMap<CustomerDTO, CustomerModel>();
            CreateMap<CustomerModel, CustomerEntity>();
            CreateMap<CustomerEntity, CustomerModel>();
            CreateMap<AccountRegisterModel, AccountEntity>();
        }
    }
}
