using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Entites
{
    public class OperationHistoryFailedEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public bool operationType { get; set; }
        public int TransactionAmount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }
        public string FailureReason { get; set; }
    }
}
