using Account.Share.Models;
using Messages.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.Share.Interfaces
{
    public interface IOperationHistoryRepository
    {
        Task UpdateFailedTransactionHistory(UpdateFailedTransaction message);
        Task UpdateSucceededTransactionHistory(UpdateSucceededTransaction message);
        List<HistoryModel> GetFilteredInfo(QueryParameters queryParameters);
        int Count(Guid accountId);
    }
}
