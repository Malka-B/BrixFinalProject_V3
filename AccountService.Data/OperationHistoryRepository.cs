using Account.Data.Entites;
using Account.Share.Interfaces;
using Account.Share.Models;
using AutoMapper;
using Messages.Commands;
using Microsoft.AspNetCore.Mvc;
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

        public List<HistoryModel> GetAll(QueryParameters queryParameters)
        {
            IQueryable<OperationHistorySucceededEntity> historyPage = _accountContext
                .SucceededOperations.Where(a => a.AccountId == queryParameters.AccountId)
                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
            if (queryParameters.HasQuery())
            {
                return GetFilteredInfo(queryParameters);
                //    historyPage = historyPage
                //        .Where(x => x.Date.ToString().Contains(queryParameters.Query.ToLowerInvariant()));
                //}
            }
            List<OperationHistorySucceededEntity> historyPage1 = historyPage
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount).ToList();
            List<HistoryModel> history = _mapper.Map<List<HistoryModel>>(historyPage1);
            return history;
        }

        public IQueryable<OperationHistorySucceededEntity> FilterByOperartionType(QueryParameters queryParameters)
        {
            var a = _accountContext.SucceededOperations.Where(t => t.AccountId == queryParameters.AccountId&& t.operationType ==queryParameters.Query.OperationType);
            a= a.OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
            return a;
        }

        public IQueryable<OperationHistorySucceededEntity> FilterByFromDate(QueryParameters queryParameters/*, DateTime fromDate*/)
        {
            return _accountContext.SucceededOperations.Where(t =>t.AccountId==queryParameters.AccountId &&t.Date <= queryParameters.Query.FromDate)
                                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
        }

        public   IQueryable<OperationHistorySucceededEntity> FilterBetweenDates(QueryParameters queryParameters)
        {
            return  _accountContext.SucceededOperations.Where(t => t.AccountId==queryParameters.AccountId&&t.Date >= queryParameters.Query.FromDate
             && t.Date <= queryParameters.Query.ToDate)
                                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
        }

        public IQueryable<OperationHistorySucceededEntity> FilterFunction(QueryParameters queryParameters)
        {
            //      IQueryable <OperationHistorySucceededEntity> operationsList;
            DateTime emptyDate = new DateTime();
            if (queryParameters.Query.FromDate == emptyDate && queryParameters.Query.ToDate == emptyDate)
                return  FilterByOperartionType(queryParameters);
            if (queryParameters.Query.FromDate != emptyDate && queryParameters.Query.ToDate != emptyDate)
                return FilterBetweenDates(queryParameters);
            else
                return FilterByFromDate(queryParameters);
        }

        public List<HistoryModel> GetFilteredInfo(QueryParameters queryParameters)
        {
            IQueryable<OperationHistorySucceededEntity> historyPage;
            if (queryParameters.HasQuery())
                historyPage = FilterFunction(queryParameters);
            else
                return GetAll(queryParameters);
            List<OperationHistorySucceededEntity> historyPage1 = historyPage
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount).ToList();
            List<HistoryModel> history = _mapper.Map<List<HistoryModel>>(historyPage1);
            return history;
        }

        public async Task UpdateFailedTransactionHistory(UpdateFailedTransaction message)
        {
            OperationHistoryFailedEntity HistoryFailedDebit = new OperationHistoryFailedEntity()
            {
                Id = new Guid(),
                AccountId = message.FromAccountId,
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
                Date = message.Date,
                operationType = true,
                TransactionAmount = message.Amount,
                TransactionId = message.TransactionId,
                FailureReason = message.FailureReason
            };
            await _accountContext.FailedOperations.AddRangeAsync(HistoryFailedDebit, HistoryFailedCredit);
            await _accountContext.SaveChangesAsync();
        }

        public async Task UpdateSucceededTransactionHistory(UpdateSucceededTransaction message)
        {
            OperationHistorySucceededEntity HistorySucceededDebit = new OperationHistorySucceededEntity()
            {
                Id = new Guid(),
                AccountId = message.FromAccountId,
                Balance = message.BalanceOfFromAccount,
                Date = message.Date,
                operationType = false,
                TransactionAmount = message.Amount,
                TransactionId = message.TransactionId
            };

            OperationHistorySucceededEntity HistorySucceededCredit = new OperationHistorySucceededEntity()
            {
                Id = new Guid(),
                AccountId = message.ToAccountId,
                Balance = message.BalanceOfToAccount,
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
