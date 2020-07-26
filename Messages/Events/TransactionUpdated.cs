using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public class TransactionUpdated
    {
        public Guid TransactionId { get; set; }
    }
}
