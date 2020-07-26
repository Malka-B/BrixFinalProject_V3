using AutoMapper;
using Tracking.WebApi.DTO;
using Transaction.Data.Entities;
using Transaction.Share.Models;

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
        }
    }
}
