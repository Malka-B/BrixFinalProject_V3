using Account.Data.Entites;
using Account.Share.Interfaces;
using Account.Share.Models;
using AutoMapper;
using Messages.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

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

        //public Task<List<HistoryModel>> GetAll(QueryParameters queryParameters)
        //{
        //   //IQueryable<Location> _allItems = _CoronaContext.Location.OrderBy(queryParameters.OrderBy,
        //   //   queryParameters.IsDescending());
        //    IQueryable<OperationHistorySucceededEntity> accountHistory = _accountContext
        //        .SucceededOperations.OrderBy(queryParameters.OrderBy,
        //     queryParameters.IsDescending());

        //    //filter by server
        //    if (queryParameters.HasQuery())
        //    {
        //        accountHistory = accountHistory
        //            .Where(x => x.Date.ToString().Contains(queryParameters.Query.ToLowerInvariant()));
        //    }

        //    HistoryModel history = _mapper.Map<HistoryModel>(accountHistory);
        //    return history
        //        .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
        //        .Take(queryParameters.PageCount);
        //}

        public IQueryable<HistoryModel> GetAll(QueryParameters queryParameters)
        {
            IQueryable<OperationHistorySucceededEntity> historyPage = _accountContext
                .SucceededOperations.Where(a => a.AccountId == queryParameters.AccountId)
                .OrderBy(queryParameters.OrderBy,queryParameters.IsDescending());

            //filter
            if (queryParameters.HasQuery())
            {
                historyPage = historyPage
                    .Where(x => x.Date.ToString().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            IQueryable<OperationHistorySucceededEntity> historyPage1 = historyPage
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            IQueryable<HistoryModel> history = _mapper.Map<IQueryable<HistoryModel>>(historyPage1);

            return history;
        }

        public async Task UpdateFailedTransactionHistory(TransactionFailed message)
        {
            AccountEntity accountDebit = await _accountContext.Accounts.FindAsync(message.FromAccountId);
            int balanceDebit = -1;
            int balanceCredit = -1;
            if (accountDebit != null)
            {
                balanceDebit = accountDebit.Balance;
            }

            AccountEntity accountCredit = await _accountContext.Accounts.FindAsync(message.ToAccountId);
            if (accountCredit != null)
            {
                balanceCredit = accountCredit.Balance;
            }

            //change do by mapper
            OperationHistoryFailedEntity HistoryFailedDebit = new OperationHistoryFailedEntity()
            {
                Id = new Guid(),
                AccountId = message.FromAccountId,
                Balance = balanceDebit,
                Date = message.Date,
                operationType = false,
                TransactionAmount = message.Amount,
                TransactionId = message.TransactionId,
                FailureReason = message.FailureReason
            };

            OperationHistoryFailedEntity HistoryFailedCredit = new OperationHistoryFailedEntity()
            {
                Id = new Guid(),
                AccountId = message.ToAccountId,
                Balance = balanceCredit,
                Date = message.Date,
                operationType = true,
                TransactionAmount = message.Amount,
                TransactionId = message.TransactionId,
                FailureReason = message.FailureReason
            };
            await _accountContext.FailedOperations.AddRangeAsync(HistoryFailedDebit, HistoryFailedCredit);
            await _accountContext.SaveChangesAsync();
        }

        public async Task UpdateSucceededTransactionHistory(TransactionSucceeded message)
        {
            AccountEntity accountDebit = await _accountContext.Accounts.FindAsync(message.FromAccountId);
            int balanceDebit = accountDebit.Balance;
            AccountEntity accountCredit = await _accountContext.Accounts.FindAsync(message.ToAccountId);
            int balanceCredit = accountCredit.Balance;
            OperationHistorySucceededEntity HistorySucceededDebit = new OperationHistorySucceededEntity()
            {
                Id = new Guid(),
                AccountId = message.FromAccountId,
                Balance = balanceDebit,
                Date = message.Date,
                operationType = false,
                TransactionAmount = message.Amount,
                TransactionId = message.TransactionId
            };

            OperationHistorySucceededEntity HistorySucceededCredit = new OperationHistorySucceededEntity()
            {
                Id = new Guid(),
                AccountId = message.ToAccountId,
                Balance = balanceCredit,
                Date = message.Date,
                operationType = true,
                TransactionAmount = message.Amount,
                TransactionId = message.TransactionId
            };
            await _accountContext.SucceededOperations.AddRangeAsync(HistorySucceededDebit, HistorySucceededCredit);
            await _accountContext.SaveChangesAsync();
        }
    }
}
