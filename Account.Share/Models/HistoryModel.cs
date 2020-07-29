using System;

namespace Account.Share.Models
{
    public class HistoryModel
    {
        public Guid AccountId { get; set; }
        public bool operationType { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }
        public Guid TransactionId { get; set; }
    }
}
