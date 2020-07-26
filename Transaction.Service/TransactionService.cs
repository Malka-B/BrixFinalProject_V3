using System;
using System.Threading.Tasks;
using Transaction.Service.Interfaces;
using Transaction.Share.Interfaces;
using Transaction.Share.Models;

namespace Transaction.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<Guid> CreateTransactionAsync(TransactionModel transactionModel)
        {
            transactionModel.Id = new Guid();
            transactionModel.Status = eStatus.proccessing;
            transactionModel.Date = DateTime.Now;
            Guid transactionId = await _transactionRepository.CreateTransactionAsync(transactionModel);   
            return transactionId;
        }
    }
}
