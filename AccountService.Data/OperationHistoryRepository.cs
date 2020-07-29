using Account.Data.Entites;
using Account.Share.Interfaces;
using Account.Share.Models;
using AutoMapper;
using Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Account.Data
{
    public class OperationHistoryRepository : IOperationHistoryRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;

        public OperationHistoryRepository(AccountContext accountContext, IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }

        public int Count(Guid accountId)
        {
            List<OperationHistorySucceededEntity> accountHistory = _accountContext.SucceededOperations
                .Where(s => s.AccountId == accountId).ToList();
            return accountHistory.Count();
        }

        public  List<HistoryModel> GetAll(QueryParameters queryParameters)
        {
            IQueryable<OperationHistorySucceededEntity> historyPage;
            if (queryParameters.OrderBy == "date")
            {
                historyPage = _accountContext
                .SucceededOperations.Where(a => a.AccountId == queryParameters.AccountId)
                .OrderByDescending(a => a.Date);
            }
            else
            {
                historyPage = _accountContext
               .SucceededOperations.Where(a => a.AccountId == queryParameters.AccountId)
               .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
            }           

            //filter
            if (queryParameters.HasQuery())
            {
                historyPage = historyPage
                    .Where(x => x.Date.ToString().Contains(queryParameters.Query.ToLowerInvariant()));                
            }

            List<OperationHistorySucceededEntity> historyPage1 = historyPage
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount).ToList();

            List<HistoryModel> history = _mapper.Map<List<HistoryModel>>(historyPage1);
            return history;
        }

        public async Task UpdateFailedTransactionHistory(UpdateFailedTransaction message)
        {        
            OperationHistoryFailedEntity operationFailedDebit = _mapper.Map<OperationHistoryFailedEntity>(message);
            operationFailedDebit.FillFields(message.FromAccountId, false);            

            OperationHistoryFailedEntity operationFailedCredit = _mapper.Map<OperationHistoryFailedEntity>(message);
            operationFailedCredit.FillFields(message.ToAccountId, true);
            
            await _accountContext.FailedOperations.AddRangeAsync(operationFailedDebit, operationFailedCredit);
            await _accountContext.SaveChangesAsync();
        }

        public async Task UpdateSucceededTransactionHistory(UpdateSucceededTransaction message)
        {      
            OperationHistorySucceededEntity operationSucceededDebit = _mapper.Map<OperationHistorySucceededEntity>(message);
            operationSucceededDebit.FillFields(message.FromAccountId, false, message.BalanceOfFromAccount);
            
            OperationHistorySucceededEntity operationSucceededCredit = _mapper.Map<OperationHistorySucceededEntity>(message);
            operationSucceededCredit.FillFields(message.ToAccountId, true, message.BalanceOfToAccount);

            await _accountContext.SucceededOperations.AddRangeAsync(operationSucceededDebit, operationSucceededCredit);
            await _accountContext.SaveChangesAsync();
        }
    }
}
