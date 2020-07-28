using Account.Share.Interfaces;
using Messages.Events;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Account.NSB
{
    public class HistoryForSuccessTransactionHandler : IHandleMessages<TransactionSucceeded>
    {
        private readonly IOperationHistoryRepository _operationHistoryRepository;

        public HistoryForSuccessTransactionHandler(IOperationHistoryRepository operationHistoryRepository)
        {
            _operationHistoryRepository = operationHistoryRepository;
        }
        public async Task Handle(TransactionSucceeded message, IMessageHandlerContext context)
        {
            await _operationHistoryRepository.UpdateSucceededTransactionHistory(message);
        }
    }
}
