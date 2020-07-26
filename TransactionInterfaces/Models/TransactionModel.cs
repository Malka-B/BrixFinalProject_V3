using System;

namespace Transaction.Share.Models
{
    public enum eStatus { proccessing, success, fail };
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
        public string FailureReason { get; set; }
        public DateTime Date { get; set; }
        public eStatus Status { get; set; }

    }
}
