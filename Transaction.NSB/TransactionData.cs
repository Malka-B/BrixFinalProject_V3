using NServiceBus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Transaction.NSB
{
    public class TransactionData: ContainSagaData
    {
        public Guid TransactionId { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
    }
}
