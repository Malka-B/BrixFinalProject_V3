﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Service.Models
{
   public class AccountRegisterModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }        
        public DateTime OpenDate { get; set; }
        public float Balance { get; set; }
    }
}
