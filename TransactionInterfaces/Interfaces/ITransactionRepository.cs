using Messages.Commands;
using System;
using System.Threading.Tasks;
using Transaction.Share.Models;

namespace Transaction.Share.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Guid> CreateTransactionAsync(TransactionModel transactionModel);
        Task<TransactionForHistory> UpdateTransactionStatusAsync(UpdateTransactionStatus updateTransactionStatus);
    }
}
