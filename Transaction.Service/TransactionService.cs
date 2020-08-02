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
       
        public async Task<TransactionDetails> CreateTransactionAsync(TransactionModel transactionModel)
        {            
            transactionModel.Status = eStatus.proccessing;
            transactionModel.Amount *= 10;
            TransactionDetails transactionId = await _transactionRepository.CreateTransactionAsync(transactionModel);            
            return transactionId;
        }
    }
}
