using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking.WebApi.DTO
{
    public class TransactionDTO
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        [Range(1,1000000)]
        public  int Amount { get; set; }
    }
}
