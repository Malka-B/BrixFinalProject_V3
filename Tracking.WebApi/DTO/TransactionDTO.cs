using System;
using System.ComponentModel.DataAnnotations;

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
