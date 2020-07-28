using System;

namespace Messages.Commands
{
    public class UpdateFailedTransaction
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid TransactionId { get; set; }
        public string FailureReason { get; set; }
    }
}
