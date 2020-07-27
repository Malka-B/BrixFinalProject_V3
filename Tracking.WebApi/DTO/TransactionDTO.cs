using System;
using System.ComponentModel.DataAnnotations;

namespace Tracking.WebApi.DTO
{
    public class TransactionDTO
    {
        [Required]
        public Guid FromAccountId { get; set; }
        [Required]
        public Guid ToAccountId { get; set; }
        [Range(1,1000000)]
        public  int Amount { get; set; }
    }
}
