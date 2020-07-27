using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Service.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string PassowrdSalt { get; set; }
    }
}
