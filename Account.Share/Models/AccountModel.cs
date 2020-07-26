using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Share.Models
{
    public class AccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OpenDate { get; set; }
        public float Balance { get; set; }
    }
}
