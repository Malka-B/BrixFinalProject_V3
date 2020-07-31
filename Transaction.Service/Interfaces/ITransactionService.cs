using System.Threading.Tasks;
using Transaction.Share.Models;

namespace Transaction.Service.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDetails> CreateTransactionAsync(TransactionModel transactionModel);
    }
}
