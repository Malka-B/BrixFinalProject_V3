using System;
using System.Collections.Generic;
using System.Text;
using Transaction.Service;
using Transaction.Share.Models;

namespace Transaction.Data.Entities
{
    public class TransactionEntity
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public eStatus Status { get; set; }
        public string FailureReason { get; set; }
    }
}
