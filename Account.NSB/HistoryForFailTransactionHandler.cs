using Account.Share.Interfaces;
using Messages.Events;
using NServiceBus;
using System;
using System.Threading.Tasks;
namespace Account.NSB
{
    public class HistoryForFailTransactionHandler : IHandleMessages<TransactionFailed>
    {
        private readonly IOperationHistoryRepository _operationHistoryRepository;

        public HistoryForFailTransactionHandler(IOperationHistoryRepository operationHistoryRepository)
        {
            _operationHistoryRepository = operationHistoryRepository;
        }
        public async Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
           await _operationHistoryRepository.UpdateFailedTransactionHistory(message);
        }
    }
}
