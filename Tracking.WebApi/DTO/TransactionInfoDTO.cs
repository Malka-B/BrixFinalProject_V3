using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction.WebApi.DTO
{
    public class TransactionInfoDTO
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}
