using Messages.Commands;
using Messages.Events;
using NServiceBus;
using System.Threading.Tasks;
using Transaction.Share.Interfaces;
using Transaction.Share.Models;

namespace Transaction.NSB
{
    public class UpdateTransactionStatusHandler : IHandleMessages<UpdateTransactionStatus>
    {
        private readonly ITransactionRepository _transactionRepository;

        public UpdateTransactionStatusHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Handle(UpdateTransactionStatus message, IMessageHandlerContext context)
        {
            TransactionForHistory transactionForHistory = await _transactionRepository.UpdateTransactionStatusAsync(message);

            if (transactionForHistory.isTransactionSucceeded)
            {
                TransactionSucceeded transactionSucceeded = new TransactionSucceeded()
                {
                    TransactionId = message.TransactionId,
                    FromAccountId = transactionForHistory.FromAccountId,
                    ToAccountId = transactionForHistory.ToAccountId,
                    Date = transactionForHistory.Date
                };
                await context.Publish(transactionSucceeded);
            }
            else
            {
                TransactionFailed transactionFailed = new TransactionFailed()
                {
                    TransactionId = message.TransactionId,
                    FromAccountId = transactionForHistory.FromAccountId,
                    ToAccountId = transactionForHistory.ToAccountId,
                    Date = transactionForHistory.Date
                };
                await context.Publish(transactionFailed);
            }           
        }
    }
}
