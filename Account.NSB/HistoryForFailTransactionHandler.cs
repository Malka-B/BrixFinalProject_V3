using Account.Share.Interfaces;
using Messages.Commands;
using NServiceBus;
using System.Threading.Tasks;
namespace Account.NSB
{
    public class HistoryForFailTransactionHandler : IHandleMessages<UpdateFailedTransaction>
    {
        private readonly IOperationHistoryRepository _operationHistoryRepository;

        public HistoryForFailTransactionHandler(IOperationHistoryRepository operationHistoryRepository)
        {
            _operationHistoryRepository = operationHistoryRepository;
        }
        public async Task Handle(UpdateFailedTransaction message, IMessageHandlerContext context)
        {
           await _operationHistoryRepository.UpdateFailedTransactionHistory(message);
        }
    }
}
