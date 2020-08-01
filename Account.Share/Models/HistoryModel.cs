using Account.Share.Enums;
using System;

namespace Account.Share.Models
{
    public class HistoryModel
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public string operationType { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }        
    }
}
