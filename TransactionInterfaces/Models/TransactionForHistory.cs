using System;

namespace Transaction.Share.Models
{
    public class TransactionForHistory
    {
        public Guid TransactionId { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public bool isTransactionSucceeded { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string FailureReason { get; set; }
    }
}
