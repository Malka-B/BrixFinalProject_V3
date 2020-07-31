using System;
using System.Collections.Generic;
using System.Text;

namespace Transaction.Share.Models
{
    public class TransactionInfoModel
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}
