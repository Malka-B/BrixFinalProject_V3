using Account.Share.Interfaces;
using Messages.Commands;
using NServiceBus;
using System.Threading.Tasks;

namespace Account.NSB
{
    public class HistoryForSuccessTransactionHandler : IHandleMessages<UpdateSucceededTransaction>
    {
        private readonly IOperationHistoryRepository _operationHistoryRepository;

        public HistoryForSuccessTransactionHandler(IOperationHistoryRepository operationHistoryRepository)
        {
            _operationHistoryRepository = operationHistoryRepository;
        }
        public async Task Handle(UpdateSucceededTransaction message, IMessageHandlerContext context)
        {
            await _operationHistoryRepository.UpdateSucceededTransactionHistory(message);
        }
    }
}
