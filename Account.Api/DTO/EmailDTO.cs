using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Account.WebApi.DTO
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
