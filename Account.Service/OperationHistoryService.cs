using Account.Service.Intefaces;
using Account.Service.Models;
using Account.Share.Interfaces;
using Account.Share.Models;
using System.Collections.Generic;

namespace Account.Service
{
    public class OperationHistoryService : IOperationHistoryService
    {
        private readonly IOperationHistoryRepository _operationHistoryRepository;

        public OperationHistoryService(IOperationHistoryRepository operationHistoryRepository)
        {
            _operationHistoryRepository = operationHistoryRepository;
        }

        public PagingReturn GetFilteredInfo(QueryParameters queryParameters)
        {
            List<HistoryModel> accountsHistory = _operationHistoryRepository
                .GetFilteredInfo(queryParameters);
            int allItemCount = _operationHistoryRepository.Count(queryParameters.AccountId);
            return new PagingReturn
            {
                AccountHistory = accountsHistory,
                Count = allItemCount
            };
        }
    }
}
