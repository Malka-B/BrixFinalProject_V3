using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Commands
{
    public class UpdateTransactionStatus
    {
        public Guid TransactionId { get; set; }
        public bool IsTransactionSucceeded { get; set; }
        public string FailureReason { get; set; }
    }
}
