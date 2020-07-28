using Account.Share.Models;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Share.Interfaces
{
    public interface IOperationHistoryRepository
    {
        Task UpdateFailedTransactionHistory(TransactionFailed message);
        Task UpdateSucceededTransactionHistory(TransactionSucceeded message);
        //Task<List<HistoryModel>> GetAll(QueryParameters queryParameters);
        IQueryable<HistoryModel> GetAll(QueryParameters queryParameters);
        int Count(Guid accountId);
    }
}
