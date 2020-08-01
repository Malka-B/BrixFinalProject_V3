using Account.Data.Entites;
using Account.Service.Models;
using Account.Share.Enums;
using Account.Share.Models;
using Account.WebApi.DTO;
using AutoMapper;
using Messages.Commands;
using System;

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
            CreateMap<OperationHistorySucceededEntity, HistoryModel>()
                .ForMember(destination => destination.Amount, option => option.MapFrom(src =>
              src.TransactionAmount))
                .ForMember(destination => destination.operationType, option => option.MapFrom(src =>
              Enum.GetName(typeof(OperationType), src.operationType)));

            //string colorName in Enum.GetNames(typeof(Colors))
            CreateMap<HistoryModel, HistoryDTO>();
            CreateMap<HistoryDTO, HistoryModel>();
            CreateMap<UpdateAccounts, TransactionDetailsForHistory>();
            CreateMap<UpdateAccounts, UpdateSucceededTransaction>();
            CreateMap<UpdateAccounts, UpdateFailedTransaction>();
            CreateMap<UpdateFailedTransaction, OperationHistoryFailedEntity>()
                .ForMember(destination => destination.TransactionAmount, option => option.MapFrom(src =>
              src.Amount));
            CreateMap<UpdateSucceededTransaction, OperationHistorySucceededEntity>()
                .ForMember(destination => destination.TransactionAmount, option => option.MapFrom(src =>
              src.Amount));          

        }
    }
}
