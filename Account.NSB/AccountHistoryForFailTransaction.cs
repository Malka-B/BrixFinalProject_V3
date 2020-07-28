using Messages.Events;
using NServiceBus;
using System;
using System.Threading.Tasks;
namespace Account.NSB
{
    public class AccountHistoryForFailTransaction : IHandleMessages<TransactionFailed>
    {
        public Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
