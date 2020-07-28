using Messages.Events;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Account.NSB
{
    public class AccountHistoryForSuccessTransaction : IHandleMessages<TransactionSucceeded>
    {
        //private readonly 
        public Task Handle(TransactionSucceeded message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
