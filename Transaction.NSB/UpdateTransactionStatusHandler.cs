using Messages.Commands;
using NServiceBus;
using System.Threading.Tasks;
using Transaction.Share.Interfaces;

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
            //TransactionForHistory transactionForHistory = 
                await _transactionRepository.UpdateTransactionStatusAsync(message);


            //if (transactionForHistory.isTransactionSucceeded)
            //{
            //    TransactionSucceeded transactionSucceeded = new TransactionSucceeded()
            //    {
            //        TransactionId = message.TransactionId,
            //        FromAccountId = transactionForHistory.FromAccountId,
            //        ToAccountId = transactionForHistory.ToAccountId,
            //        Date = transactionForHistory.Date,
            //        Amount = transactionForHistory.Amount
            //    };
            //    await context.Publish(transactionSucceeded);
            //}
            //else
            //{
            //    TransactionFailed transactionFailed = new TransactionFailed()
            //    {
            //        TransactionId = message.TransactionId,
            //        FromAccountId = transactionForHistory.FromAccountId,
            //        ToAccountId = transactionForHistory.ToAccountId,
            //        Date = transactionForHistory.Date,
            //        Amount = transactionForHistory.Amount,
            //        FailureReason = transactionForHistory.FailureReason
            //    };
            //    await context.Publish(transactionFailed);
            //}
            await Task.CompletedTask;
        }
    }
}
