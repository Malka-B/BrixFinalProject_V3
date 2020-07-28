using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Entites
{
    public class OperationHistorySucceededEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public bool CreditOrDebit { get; set; }
        public int TransactionAmount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }
    }
}
