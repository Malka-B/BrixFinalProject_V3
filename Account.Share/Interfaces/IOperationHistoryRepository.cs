using Account.Share.Models;
using Messages.Commands;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Share.Interfaces
{
    public interface IOperationHistoryRepository
    {
        Task UpdateFailedTransactionHistory(UpdateFailedTransaction message);
        Task UpdateSucceededTransactionHistory(UpdateSucceededTransaction message);        
        List<HistoryModel> GetAll(QueryParameters queryParameters);
        int Count(Guid accountId);
    }
}
