using System;

namespace Account.Share.Models
{
    public class TransactionDetailsForHistory
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
        public int BalanceOfFromAccount { get; set; }
        public int BalanceOfToAccount { get; set; }
        public string FailureReason { get; set; }
        public DateTime Date { get; set; }
        public Guid TransactionId { get; set; }
    }
}
