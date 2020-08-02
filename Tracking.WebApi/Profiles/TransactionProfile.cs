using AutoMapper;
using Messages.Commands;
using Messages.Events;
using Tracking.WebApi.DTO;
using Transaction.Data.Entities;
using Transaction.Share.Models;
using Transaction.WebApi.DTO;

namespace Tracking.WebApi.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionModel, TransactionDTO>();
            CreateMap<TransactionDTO, TransactionModel>();
            CreateMap<TransactionEntity, TransactionModel>();
            CreateMap<TransactionModel, TransactionEntity>();
            CreateMap<TransactionEntity, TransactionInfoModel>();
            CreateMap<TransactionInfoModel, TransactionInfoDTO>();
            CreateMap<AccountsUpdated, UpdateTransactionStatus>()
                .ForMember(destination => destination.IsTransactionSucceeded, option => 
                option.MapFrom(src =>(src.isAccountsUpdateSuccess)));

        }
    }
}
