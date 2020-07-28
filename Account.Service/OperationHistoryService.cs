using Account.Service.Intefaces;
using Account.Service.Models;
using Account.Share.Interfaces;
using Account.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Service
{
    public class OperationHistoryService : IOperationHistoryService
    {
        private readonly IOperationHistoryRepository _operationHistoryRepository;

        public OperationHistoryService(IOperationHistoryRepository operationHistoryRepository)
        {
            _operationHistoryRepository = operationHistoryRepository;
        }

        public PagingReturn GetAll(QueryParameters queryParameters)
        {
            List<HistoryModel> accountHistory = _operationHistoryRepository
                .GetAll(queryParameters)
                .ToList();

            int allItemCount =  _operationHistoryRepository.Count(queryParameters.AccountId);

            return new PagingReturn
            {
                AccountHistory = accountHistory,
                Count = allItemCount
            };
        }
    }
}
