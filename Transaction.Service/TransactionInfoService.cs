using System;
using System.Threading.Tasks;
using Transaction.Service.Interfaces;
using Transaction.Share.Models;

namespace Transaction.Service
{
    public class TransactionInfoService : ITransactionInfoService
    {
        private readonly ITransactionInfoRepository _transactionInfoRepository;
        public TransactionInfoService(ITransactionInfoRepository transactionInfoRepository)
        {
            _transactionInfoRepository = transactionInfoRepository;
        }
        public async Task<TransactionInfoModel> GetTransactionInfoAsync(Guid transactionId)
        {
            var a= await _transactionInfoRepository.GetTransactionInfoAsync(transactionId);
            return a; 
        }
    }
}
